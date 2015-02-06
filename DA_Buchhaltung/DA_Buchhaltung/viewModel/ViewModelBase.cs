/*
 * Klasse: ViewModelBase.cs
 * Author: Martin Osterwalder
 * Implementierung der INotifyPropertyChanged Schnittstelle.
 * Zusätzliches Fremdsnippet für die Validierung mit Annotations. Source von Technet (Detaillink siehe weiter unten)
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DA_Buchhaltung.viewModel
{
    public class ViewModelBase : INotifyPropertyChanged, IDataErrorInfo
    {

        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        // von http://social.technet.microsoft.com/wiki/contents/articles/22660.data-validation-in-mvvm.aspx
        #region ValidationSnippet
        string IDataErrorInfo.Error
        {
            get
            {
                throw new NotSupportedException("IDataErrorInfo.Error is not supported, use IDataErrorInfo.this[propertyName] instead.");
            }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                return OnValidate(propertyName);
            }
        }


        /// <summary>
        /// Sets the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertySelector">Expression tree contains the property definition.</param>
        /// <param name="value">The property value.</param>
        protected void SetValue<T>(Expression<Func<T>> propertySelector, T value)
        {
            string propertyName = GetPropertyName(propertySelector);

            SetValue<T>(propertyName, value);
        }

        /// <summary>
        /// Sets the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="value">The property value.</param>
        protected void SetValue<T>(string propertyName, T value)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            _values[propertyName] = value;
            OnPropertyChanged(propertyName);
        }
        private string GetPropertyName(LambdaExpression expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException();
            }

            return memberExpression.Member.Name;
        }

        /// <summary>
        /// Gets the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertySelector">Expression tree contains the property definition.</param>
        /// <returns>The value of the property or default value if not exist.</returns>
        protected T GetValue<T>(Expression<Func<T>> propertySelector)
        {
            string propertyName = GetPropertyName(propertySelector);

            return GetValue<T>(propertyName);
        }

        /// <summary>
        /// Gets the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value of the property or default value if not exist.</returns>
        protected T GetValue<T>(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            object value;
            if (!_values.TryGetValue(propertyName, out value))
            {
                value = default(T);
                _values.Add(propertyName, value);
            }

            return (T)value;
        }

        /// <summary>
        /// Validates current instance properties using Data Annotations.
        /// </summary>
        /// <param name="propertyName">This instance property to validate.</param>
        /// <returns>Relevant error string on validation failure or <see cref="System.String.Empty"/> on validation success.</returns>
        protected virtual string OnValidate(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            string error = string.Empty;
            var value = GetValue(propertyName);
            var results = new List<ValidationResult>(1);
            var result = Validator.TryValidateProperty(
                value,
                new ValidationContext(this, null, null)
                {
                    MemberName = propertyName
                },
                results);

            if (!result)
            {
                var validationResult = results.First();
                error = validationResult.ErrorMessage;
            }

            return error;
        }

        private object GetValue(string propertyName)
       {
           object value;
           if (!_values.TryGetValue(propertyName, out value))
           {
               var propertyDescriptor = TypeDescriptor.GetProperties(GetType()).Find(propertyName, false);
               if (propertyDescriptor == null)
               {
                   throw new ArgumentException("Invalid property name", propertyName);
               }
 
               value = propertyDescriptor.GetValue(this);
               _values.Add(propertyName, value);
           }
 
           return value;
       }
 
       #region Debugging
 
       /// <summary>
       /// Warns the developer if this object does not have
       /// a public property with the specified name. This
       /// method does not exist in a Release build.
       /// </summary>
       [Conditional("DEBUG")]
       [DebuggerStepThrough]
       public void VerifyPropertyName(string propertyName)
       {
           // Verify that the property name matches a real, 
           // public, instance property on this object.
           if (TypeDescriptor.GetProperties(this)[propertyName] == null)
           {
               string msg = "Invalid property name: " + propertyName;
 
               if (this.ThrowOnInvalidPropertyName)
                   throw new Exception(msg);
               else
                   Debug.Fail(msg);
           }
       }
 
       /// <summary>
       /// Returns whether an exception is thrown, or if a Debug.Fail() is used
       /// when an invalid property name is passed to the VerifyPropertyName method.
       /// The default value is false, but subclasses used by unit tests might
       /// override this property's getter to return true.
       /// </summary>
       protected virtual bool ThrowOnInvalidPropertyName { get; private set; }
       #endregion
        #endregion
    }
}
