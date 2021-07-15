
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Linq.Expressions;
using System.Linq;

namespace Mvvm.Core
{
   public class ViewModelBase : INotifyPropertyChanged, INotifyPropertyChanging
   {
      public event PropertyChangedEventHandler PropertyChanged;
      public event PropertyChangingEventHandler PropertyChanging;

      public void VerifyPropertyName(string propertyName)
      {
         var myType = GetType();

         if (!string.IsNullOrEmpty(propertyName)
             && myType.GetProperty(propertyName) == null)
         {
            var descriptor = this as ICustomTypeDescriptor;

            if (descriptor != null)
            {
               if (descriptor.GetProperties()
                   .Cast<PropertyDescriptor>()
                   .Any(property => property.Name == propertyName))
               {
                  return;
               }
            }

            throw new ArgumentException("Property not found", propertyName);
         }
      }

      public virtual void RaisePropertyChanging(string propertyName)
      {
         VerifyPropertyName(propertyName);
         PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
      }


      public virtual void RaisePropertyChanged(string propertyName)
      {
         VerifyPropertyName(propertyName);
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

      public virtual void RaisePropertyChanging<T>(Expression<Func<T>> propertyExpression)
      {
         var handler = PropertyChanging;
         if (handler != null)
         {
            var propertyName = GetPropertyName(propertyExpression);
            handler(this, new PropertyChangingEventArgs(propertyName));
         }
      }

      public virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
      {
         var handler = PropertyChanged;
         if (handler != null)
         {
            var propertyName = GetPropertyName(propertyExpression);

            if (!string.IsNullOrEmpty(propertyName))
               RaisePropertyChanged(propertyName);
         }
      }

      protected static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
      {
         if (propertyExpression == null)
            throw new ArgumentNullException(nameof(propertyExpression));


         if (!(propertyExpression.Body is MemberExpression body))
            throw new ArgumentException("Invalid argument", nameof(propertyExpression));

         var property = body.Member as PropertyInfo;

         if (property == null)
            throw new ArgumentException("Argument is not a property", nameof(propertyExpression));

         return property.Name;
      }

      protected bool Set<T>(Expression<Func<T>> propertyExpression, ref T field, T newValue)
      {
         if (EqualityComparer<T>.Default.Equals(field, newValue))
         {
            return false;
         }

         RaisePropertyChanging(propertyExpression);
         field = newValue;
         RaisePropertyChanged(propertyExpression);
         return true;
      }

      protected bool Set<T>(string propertyName, ref T field, T newValue)
      {
         if (EqualityComparer<T>.Default.Equals(field, newValue))
         {
            return false;
         }

         RaisePropertyChanging(propertyName);
         field = newValue;
         RaisePropertyChanged(propertyName);

         return true;
      }
   }
}
