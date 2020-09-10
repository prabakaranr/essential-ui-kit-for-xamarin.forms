using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace EssentialUIKit.Behaviors.ECommerce
{
    /// <summary>
    /// This class extends the behavior of the Expander control to achieve event to command behavior.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ExpanderBehavior : Behavior<Expander>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the CommandProperty, and it is a bindable property.
        /// </summary>
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ExpanderBehavior));

        /// <summary>
        /// Gets or sets the CommandParameterProperty, and it is a bindable property.
        /// </summary>
        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ExpanderBehavior));

        /// <summary>
        /// Gets or sets the Command.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the CommandParameter.
        /// </summary>
        public object CommandParameter
        {
            get { return this.GetValue(CommandParameterProperty); }
            set { this.SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// Gets the Expander.
        /// </summary>
        public Expander Expander { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when adding Expander to view.
        /// </summary>
        /// <param name="expander">The SfExpander</param>
        protected override void OnAttachedTo(Expander expander)
        {
            if (expander != null)
            {
                base.OnAttachedTo(expander);
                this.Expander = expander;
                expander.BindingContextChanged += this.OnBindingContextChanged;
                expander.PropertyChanged += Expander_PropertyChanged;
            }
        }

        private void Expander_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == Expander.IsExpandedProperty.PropertyName && Expander.IsExpanded)
            {
                OnExpanding();
            }
        }

        /// <summary>
        /// Invoked when exit from the view.
        /// </summary>
        /// <param name="expander">The SfExpander</param>
        protected override void OnDetachingFrom(Expander expander)
        {
            if (expander != null)
            {
                base.OnDetachingFrom(expander);
                expander.BindingContextChanged -= this.OnBindingContextChanged;
                expander.PropertyChanged -= Expander_PropertyChanged;
                this.Expander = null;
            }
        }

        /// <summary>
        /// Invoked when expander binding context is changed.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            this.BindingContext = this.Expander.BindingContext;
        }

        /// <summary>
        /// Invoked when expander is expanded.
        /// </summary>
        /// <param name="sender">The SfExpander</param>
        /// <param name="e">The expanding and collapsing event args</param>
        private void OnExpanding()
        {
            if (this.Command == null)
            {
                return;
            }

            var parameter = new System.Collections.Generic.List<object>
            {
                Expander.BindingContext, this.CommandParameter
            };

            if (this.Command.CanExecute(parameter))
            {
                this.Command.Execute(parameter);
            }
        }

        /// <summary>
        /// Invoked when expander binding context is changed.
        /// </summary>
        /// <param name="sender">The SfExpander</param>
        /// <param name="e">The event args</param>
        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            this.OnBindingContextChanged();
        }

        #endregion
    }
}