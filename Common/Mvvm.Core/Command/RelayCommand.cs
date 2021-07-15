using System;
using System.Windows.Input;
using Mvvm.Core.Helpers;

namespace Mvvm.Core.Command
{
   public class RelayCommand : ICommand
   {
      private readonly WeakAction execute;
      private readonly WeakFunc<bool> canExecute;

      private EventHandler requerySuggestedLocal;

      public RelayCommand(Action execute, bool keepTargetAlive = false)
          : this(execute, null, keepTargetAlive)
      {
      }

      public RelayCommand(Action execute, Func<bool> canExecute, bool keepTargetAlive = false)
      {
         if (execute == null)
            throw new ArgumentNullException(nameof(execute));

         this.execute = new WeakAction(execute, keepTargetAlive);

         if (canExecute != null)
            this.canExecute = new WeakFunc<bool>(canExecute, keepTargetAlive);
      }


      public event EventHandler CanExecuteChanged
      {
         add
         {
            if (canExecute != null)
            {
               // add event handler to local handler backing field in a thread safe manner
               EventHandler handler2;
               var canExecuteChanged = requerySuggestedLocal;

               do
               {
                  handler2 = canExecuteChanged;
                  var handler3 = (EventHandler)Delegate.Combine(handler2, value);
                  canExecuteChanged = System.Threading.Interlocked.CompareExchange(
                      ref requerySuggestedLocal,
                      handler3,
                      handler2);
               }
               while (canExecuteChanged != handler2);

               CommandManager.RequerySuggested += value;
            }
         }

         remove
         {
            if (canExecute != null)
            {
               // removes an event handler from local backing field in a thread safe manner
               EventHandler handler2;
               var canExecuteChanged = requerySuggestedLocal;

               do
               {
                  handler2 = canExecuteChanged;
                  var handler3 = (EventHandler)Delegate.Remove(handler2, value);

                  canExecuteChanged = System.Threading.Interlocked.CompareExchange(
                      ref requerySuggestedLocal,
                      handler3,
                      handler2);
               }
               while (canExecuteChanged != handler2);

               CommandManager.RequerySuggested -= value;
            }
         }
      }

      public void RaiseCanExecuteChanged()
      {
         CommandManager.InvalidateRequerySuggested();
      }

      public bool CanExecute(object parameter)
      {
         return canExecute == null || (canExecute.IsStatic || canExecute.IsAlive) && canExecute.Execute();
      }

      public virtual void Execute(object parameter)
      {
         if (CanExecute(parameter) && execute != null && (execute.IsStatic || execute.IsAlive))
            execute.Execute();
      }
   }
}