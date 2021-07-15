using System;
using System.Windows.Input;
using Mvvm.Core.Helpers;

namespace Mvvm.Core.Command
{
   public class RelayCommand<T> : ICommand
   {
      private readonly WeakAction<T> execute;

      private readonly WeakFunc<T, bool> canExecute;

      public RelayCommand(Action<T> execute, bool keepTargetAlive = false)
          : this(execute, null, keepTargetAlive)
      {
      }

      public RelayCommand(Action<T> execute, Func<T, bool> canExecute, bool keepTargetAlive = false)
      {
         if (execute == null)
            throw new ArgumentNullException(nameof(execute));

         this.execute = new WeakAction<T>(execute, keepTargetAlive);

         if (canExecute != null)
            this.canExecute = new WeakFunc<T, bool>(canExecute, keepTargetAlive);
      }

      public event EventHandler CanExecuteChanged
      {
         add
         {
            if (canExecute != null)
               CommandManager.RequerySuggested += value;
         }

         remove
         {
            if (canExecute != null)
               CommandManager.RequerySuggested -= value;
         }
      }

      public void RaiseCanExecuteChanged()
      {
         CommandManager.InvalidateRequerySuggested();
      }

      public bool CanExecute(object parameter)
      {
         if (canExecute == null)
         {
            return true;
         }

         if (canExecute.IsStatic || canExecute.IsAlive)
         {
            if (parameter == null && typeof(T).IsValueType)
               return canExecute.Execute(default);

            if (parameter == null || parameter is T)
               return canExecute.Execute((T)parameter);
         }

         return false;
      }

      public virtual void Execute(object parameter)
      {
         var val = parameter;

         if (parameter != null
             && parameter.GetType() != typeof(T))
         {
            if (parameter is IConvertible)
            {
               val = Convert.ChangeType(parameter, typeof(T), null);
            }
         }

         if (CanExecute(val) && execute != null && (execute.IsStatic || execute.IsAlive))
         {
            if (val == null)
            {
               if (typeof(T).IsValueType)
                  execute.Execute(default);
               else
                  execute.Execute((T)val);
            }
            else
            {
               execute.Execute((T)val);
            }
         }
      }
   }
}