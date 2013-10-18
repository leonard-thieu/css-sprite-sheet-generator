﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CssSpriteSheetGenerator.Gui.Properties;
using CssSpriteSheetGenerator.Gui.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Drawing = System.Drawing;

namespace CssSpriteSheetGenerator.Gui
{
    /// <summary>
    ///     <para>Helper methods for <see cref="CssSpriteSheetGenerator.Gui" />.</para>
    ///     <para>NOTE: This class is in the Infrastructure subfolder but in the <see cref="CssSpriteSheetGenerator.Gui" /> namespace.</para>
    /// </summary>
    public static class Helper
    {
        #region Extension Methods

        #region WPF Extension Methods

        #region BitmapImage Extensions

        /// <summary>
        /// Converts a <see cref="BitmapSource" /> to a <see cref="Drawing.Bitmap" />.
        /// </summary>
        /// <param name="bitmapSource">The <see cref="BitmapSource" /> to convert.</param>
        /// <returns>The converted <see cref="Drawing.Bitmap" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="bitmapSource" /> cannot be null.</exception>
        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public static Drawing.Bitmap ToDrawingBitmap(this BitmapSource bitmapSource)
        {
            if (bitmapSource == null)
                throw new ArgumentNullException("bitmapSource");

            using (var stream = new MemoryStream())
            {
                var encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(stream);
                Drawing.Bitmap bitmap = new Drawing.Bitmap(stream);

                return new Drawing.Bitmap(bitmap);
            }
        }

        #endregion

        #region DependencyObject Extensions

        /// <summary>
        /// Finds the visual child of type <typeparamref name="T" /> on
        /// <paramref name="obj" />.
        /// </summary>
        /// <typeparam name="T">The type of the element to find. Must derive from <see cref="DependencyObject" />.</typeparam>
        /// <param name="obj">The dependency object to search.</param>
        /// <returns>
        /// If found, the visual child of the specified type on the dependency object.
        /// Otherwise, null.
        /// </returns>
        [ExcludeFromCodeCoverage]
        public static T FindVisualChild<T>(this DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        #endregion

        #region Point Extensions

        /// <summary>
        /// Clamps a <see cref="Point" /> value to the range of {0,0} to <paramref name="max" />.
        /// </summary>
        /// <param name="value">The <see cref="Point" /> value to clamp.</param>
        /// <param name="max">The maximum value to clamp to.</param>
        /// <returns>The clamped value.</returns>
        public static Point Clamp(this Point value, Point max)
        {
            var x = Clamp(value.X, 0, max.X);
            var y = Clamp(value.Y, 0, max.Y);

            return new Point(x, y);
        }

        /// <summary>
        /// Clamps a <see cref="Point" /> value to the range of {0,0} to <paramref name="max" />.
        /// </summary>
        /// <param name="value">The <see cref="Point" /> value to clamp.</param>
        /// <param name="max">The maximum value to clamp to.</param>
        /// <returns>The clamped value.</returns>
        public static Point Clamp(this Point value, Size max)
        {
            return value.Clamp((Point)max);
        }

        /// <summary>
        /// Offsets a <see cref="Point" /> by the specified amount.
        /// </summary>
        /// <param name="point">The point to offset.</param>
        /// <param name="offset">The amount to offset the point by.</param>
        /// <returns>The offset point.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", MessageId = "1#")]
        public static Point Offset(this Point point, Vector offset)
        {
            return point + offset;
        }

        /// <summary>
        /// Offsets a <see cref="Point" /> by the specified amount.
        /// </summary>
        /// <param name="point">The point to offset.</param>
        /// <param name="offset">The amount to offset the point by.</param>
        /// <returns>The offset point.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", MessageId = "1#")]
        public static Point Offset(this Point point, Size offset)
        {
            return point + (Vector)offset;
        }

        /// <summary>
        /// Converts a <see cref="Point" /> value to a <see cref="Drawing.Point" /> value.
        /// </summary>
        /// <param name="point">The <see cref="Point" /> value to convert.</param>
        /// <returns>The equivalent <see cref="Drawing.Point" /> value.</returns>
        public static Drawing.Point ToDrawingPoint(this Point point)
        {
            return new Drawing.Point((int)point.X, (int)point.Y);
        }

        /// <summary>
        /// Converts a <see cref="Point" /> value to a <see cref="Thickness" /> value.
        /// </summary>
        /// <param name="point">The <see cref="Point" /> value to convert.</param>
        /// <returns>The equivalent <see cref="Thickness" /> value.</returns>
        public static Thickness ToThickness(this Point point)
        {
            return new Thickness(point.X, point.Y, 0, 0);
        }

        #endregion

        #region Rect Extensions

        /// <summary>
        /// Converts a <see cref="Rect" /> value to a <see cref="Drawing.Rectangle" /> value.
        /// </summary>
        /// <param name="rect">The <see cref="Rect" /> value to convert.</param>
        /// <returns>The equivalent <see cref="Drawing.Rectangle" /> value.</returns>
        public static Drawing.Rectangle ToDrawingRectangle(this Rect rect)
        {
            return new Drawing.Rectangle(rect.Location.ToDrawingPoint(), rect.Size.ToDrawingSize());
        }

        #endregion

        #region Size Extensions

        /// <summary>
        /// Converts a <see cref="Size" /> value to a <see cref="Drawing.Size" /> value.
        /// </summary>
        /// <param name="size">The <see cref="Size" /> value to convert.</param>
        /// <returns>The equivalent <see cref="Drawing.Size" /> value.</returns>
        public static Drawing.Size ToDrawingSize(this Size size)
        {
            return new Drawing.Size((int)size.Width, (int)size.Height);
        }

        #endregion

        #region Thickness Extensions

        /// <summary>
        /// Returns <paramref name="thickness" /> with its <see cref="Thickness.Left" /> changed to <paramref name="left" />.
        /// </summary>
        /// <param name="thickness">The <see cref="Thickness" /> value to copy from.</param>
        /// <param name="left">The new value for the <see cref="Thickness.Left" /> property.</param>
        /// <returns>A <see cref="Thickness" /> with its <see cref="Thickness.Left" /> changed to <paramref name="left" />.</returns>
        public static Thickness SetLeft(this Thickness thickness, double left)
        {
            return new Thickness(left, thickness.Top, thickness.Right, thickness.Bottom);
        }

        /// <summary>
        /// Returns <paramref name="thickness" /> with its <see cref="Thickness.Top" /> changed to <paramref name="top" />.
        /// </summary>
        /// <param name="thickness">The <see cref="Thickness" /> value to copy from.</param>
        /// <param name="top">The new value for the <see cref="Thickness.Top" /> property.</param>
        /// <returns>A <see cref="Thickness" /> with its <see cref="Thickness.Top" /> changed to <paramref name="top" />.</returns>
        public static Thickness SetTop(this Thickness thickness, double top)
        {
            return new Thickness(thickness.Left, top, thickness.Right, thickness.Bottom);
        }

        /// <summary>
        /// Returns <paramref name="thickness" /> with its <see cref="Thickness.Right" /> changed to <paramref name="right" />.
        /// </summary>
        /// <param name="thickness">The <see cref="Thickness" /> value to copy from.</param>
        /// <param name="right">The new value for the <see cref="Thickness.Right" /> property.</param>
        /// <returns>A <see cref="Thickness" /> with its <see cref="Thickness.Right" /> changed to <paramref name="right" />.</returns>
        public static Thickness SetRight(this Thickness thickness, double right)
        {
            return new Thickness(thickness.Left, thickness.Top, right, thickness.Bottom);
        }

        /// <summary>
        /// Returns <paramref name="thickness" /> with its <see cref="Thickness.Bottom" /> changed to <paramref name="bottom" />.
        /// </summary>
        /// <param name="thickness">The <see cref="Thickness" /> value to copy from.</param>
        /// <param name="bottom">The new value for the <see cref="Thickness.Bottom" /> property.</param>
        /// <returns>A <see cref="Thickness" /> with its <see cref="Thickness.Bottom" /> changed to <paramref name="bottom" />.</returns>
        public static Thickness SetBottom(this Thickness thickness, double bottom)
        {
            return new Thickness(thickness.Left, thickness.Top, thickness.Right, bottom);
        }

        /// <summary>
        /// Converts a <see cref="Thickness" /> value to a <see cref="Point" /> value.
        /// </summary>
        /// <param name="thickness">The <see cref="Thickness" /> value to convert.</param>
        /// <returns>The equivalent <see cref="Point" /> value.</returns>
        public static Point ToPoint(this Thickness thickness)
        {
            return new Point(thickness.Left, thickness.Top);
        }

        #endregion

        #region UIElement Extensions

        /// <summary>
        /// Removes all adorners that are bound to <paramref name="uiElement" />.
        /// </summary>
        /// <param name="uiElement">The element to remove adorners from.</param>
        [ExcludeFromCodeCoverage]
        public static void ClearAdorners(this UIElement uiElement)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(uiElement);
            var adorners = adornerLayer.GetAdorners(uiElement);
            if (adorners != null)
                foreach (var adorner in adorners)
                    adornerLayer.Remove(adorner);
        }

        #endregion

        #region Vector Extensions

        /// <summary>
        /// Flips the x value of a <see cref="Vector" />.
        /// </summary>
        /// <param name="vector">The vector to modify.</param>
        /// <returns>A vector with a flipped x value.</returns>
        public static Vector FlipX(this Vector vector)
        {
            return new Vector(-vector.X, vector.Y);
        }

        /// <summary>
        /// Flips the y value of a <see cref="Vector" />.
        /// </summary>
        /// <param name="vector">The vector to modify.</param>
        /// <returns>A vector with a flipped y value.</returns>
        public static Vector FlipY(this Vector vector)
        {
            return new Vector(vector.X, -vector.Y);
        }

        #endregion

        #region Window Extensions

        /// <summary>
        /// Displays a dialog of type <typeparamref name="T" /> and executes the callback
        /// using the filename returned as a parameter.
        /// </summary>
        /// <typeparam name="T">The type of dialog to open. Must derive from <see cref="FileDialog" />.</typeparam>
        /// <param name="owner">Handle to the window that owns the dialog.</param>
        /// <param name="defaultExt"></param>
        /// <param name="filter">
        /// The filter string that determines what types of files are displayed from either the
        /// <see cref="OpenFileDialog" /> or <see cref="SaveFileDialog" />.
        /// </param>
        /// <param name="callback">The callback to execute.</param>
        /// <exception cref="ArgumentNullException"><paramref name="callback" /> cannot be null.</exception>
        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void ExecuteDialog<T>(this Window owner, string defaultExt, string filter, Action<string> callback) where T : FileDialog
        {
            if (callback == null)
                throw new ArgumentNullException("callback");

            var dialog = (FileDialog)Activator.CreateInstance<T>();
            dialog.DefaultExt = defaultExt;
            dialog.Filter = filter;
            if ((bool)dialog.ShowDialog(owner))
                callback(dialog.FileName);
        }

        #endregion

        #endregion

        #region System.Drawing Extension Methods

        #region Drawing.Point Extensions

        /// <summary>
        /// Converts a <see cref="Drawing.Point" /> value to a <see cref="Point" /> value.
        /// </summary>
        /// <param name="point">The <see cref="Drawing.Point" /> value to convert.</param>
        /// <returns>The equivalent <see cref="Point" /> value.</returns>
        public static Point ToWpfPoint(this Drawing.Point point)
        {
            return new Point(point.X, point.Y);
        }

        #endregion

        #region Drawing.Rectangle Extensions

        /// <summary>
        /// Converts a <see cref="Drawing.Rectangle" /> value to a <see cref="Rect" /> value.
        /// </summary>
        /// <param name="rect">The <see cref="Drawing.Rectangle" /> value to convert.</param>
        /// <returns>The equivalent <see cref="Rect" /> value.</returns>
        public static Rect ToWpfRect(this Drawing.Rectangle rect)
        {
            return new Rect(rect.Location.ToWpfPoint(), rect.Size.ToWpfSize());
        }

        #endregion

        #region Drawing.Size Extensions

        /// <summary>
        /// Converts a <see cref="Drawing.Size" /> value to a <see cref="Size" /> value.
        /// </summary>
        /// <param name="size">The <see cref="Drawing.Size" /> value to convert.</param>
        /// <returns>The equivalent <see cref="Size" /> value.</returns>
        public static Size ToWpfSize(this Drawing.Size size)
        {
            return new Size(size.Width, size.Height);
        }

        #endregion

        #endregion

        #region Miscellaneous Extension Methods

        #region double Extensions

        /// <summary>
        /// Converts a double value to an int value.
        /// </summary>
        /// <param name="value">The double value to convert.</param>
        /// <returns>The converted int value.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "int")]
        public static int ToInt(this double value)
        {
            return (int)(value + 0.5);
        }

        #endregion

        #region Exception Extensions

        /// <summary>
        /// Gets all exception messages.
        /// </summary>
        /// <param name="exception">The exception to get messages from.</param>
        /// <returns>All of the exception messages from <paramref name="exception" />, each on its own line.</returns>
        public static string GetAllMessages(this Exception exception)
        {
            var messages = exception.FromHierarchy(ex => ex.InnerException)
                .Select(ex => ex.Message);
            return string.Join(Environment.NewLine, messages);
        }

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        [ExcludeFromCodeCoverage]
        public static void Log(this Exception exception)
        {
            var messages = exception.GetAllMessages();
            Debug.WriteLine(messages);
        }

        /// <summary>
        /// Logs and notifies subscribers of an exception.
        /// </summary>
        /// <param name="exception">The exception to notify about.</param>
        public static void LogAndSendExceptionNotification(this Exception exception)
        {
            var messages = exception.GetAllMessages();
            Debug.WriteLine(messages);
            SendMessage<string>(messages, "Exception");
        }

        #endregion

        #region ISpriteSheetViewModel Extensions

        /// <summary>
        /// If the sprite sheet is modified, prompts the user if they want to save first.
        ///     If yes, saves and then calls <paramref name="execute" />.
        ///     If no, calls <paramref name="execute" />.
        ///     Else, does nothing.
        /// If the sprite sheet isn't modified, calls <paramref name="execute" />.
        /// </summary>
        /// <param name="spriteSheetViewModel">The sprite sheet view model.</param>
        /// <param name="execute">The action to execute.</param>
        /// <exception cref="ArgumentNullException"><paramref name="spriteSheetViewModel" /> cannot be null.</exception>
        public static void PromptSaveExecute(this ISpriteSheetViewModel spriteSheetViewModel, Action execute)
        {
            PromptSaveExecute(spriteSheetViewModel, execute, null);
        }

        /// <summary>
        /// If the sprite sheet is modified, prompts the user if they want to save first.
        ///     If yes, saves and then calls <paramref name="execute" />.
        ///     If no, calls <paramref name="execute" />.
        ///     Else, calls <paramref name="cancel" />.
        /// If the sprite sheet isn't modified, calls <paramref name="execute" />.
        /// </summary>
        /// <param name="spriteSheetViewModel">The sprite sheet view model.</param>
        /// <param name="execute">The action to execute.</param>
        /// <param name="cancel">The action to execute if the user cancels.</param>
        /// <exception cref="ArgumentNullException"><paramref name="spriteSheetViewModel" /> cannot be null.</exception>
        public static void PromptSaveExecute(this ISpriteSheetViewModel spriteSheetViewModel, Action execute, Action cancel)
        {
            if (spriteSheetViewModel == null)
                throw new ArgumentNullException("spriteSheetViewModel");

            if (spriteSheetViewModel.IsModified)
                Messenger.Default.Send<DialogMessage>(
                    new DialogMessage(
                        string.Format(CultureInfo.CurrentUICulture, "Do you want to save changes to {0}?", spriteSheetViewModel.FileName),
                        r =>
                        {
                            switch (r)
                            {
                                case MessageBoxResult.Yes:
                                    spriteSheetViewModel.Export.Execute();
                                    execute();
                                    break;
                                case MessageBoxResult.No:
                                    execute();
                                    break;
                                case MessageBoxResult.Cancel:
                                    if (cancel != null)
                                        cancel();
                                    break;
                            }
                        }) { Caption = Resources.ApplicationName });
            else
                execute();
        }

        #endregion

        #region RelayCommand Extensions

        /// <summary>
        /// Short-hand for <see cref="GalaSoft.MvvmLight.Command.RelayCommand.CanExecute" />.
        /// </summary>
        /// <param name="relayCommand">The relay command to call <see cref="GalaSoft.MvvmLight.Command.RelayCommand.CanExecute" /> on.</param>
        /// <returns>The result of <see cref="GalaSoft.MvvmLight.Command.RelayCommand.CanExecute" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="relayCommand" /> cannot be null.</exception>
        public static bool CanExecute(this RelayCommand relayCommand)
        {
            if (relayCommand == null)
                throw new ArgumentNullException("relayCommand");

            return relayCommand.CanExecute(null);
        }

        /// <summary>
        /// Short-hand for <see cref="GalaSoft.MvvmLight.Command.RelayCommand.Execute" />.
        /// </summary>
        /// <param name="relayCommand">The relay command to call <see cref="GalaSoft.MvvmLight.Command.RelayCommand.Execute" /> on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="relayCommand" /> cannot be null.</exception>
        public static void Execute(this RelayCommand relayCommand)
        {
            if (relayCommand == null)
                throw new ArgumentNullException("relayCommand");

            relayCommand.Execute(null);
        }

        #endregion

        #region ViewModelBase Extensions

        /// <summary>
        /// Returns a <see cref="System.Windows.Input.InputGestureCollection" /> with the specified input gestures.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="inputGestures">The input gestures.</param>
        /// <returns>A <see cref="System.Windows.Input.InputGestureCollection" /> with the specified input gestures.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "viewModel")]
        public static InputGestureCollection GetInputGestures(this ViewModelBase viewModel, params string[] inputGestures)
        {
            return new InputGestureCollection(inputGestures.Select(i => Helper.Get<KeyGesture>(i)).ToList());
        }

        #endregion

        #endregion

        #endregion

        #region Helpers

        #region General Purpose Helpers

        /// <summary>
        /// Clamps <paramref name="value" /> to the range of <paramref name="min" /> to
        /// <paramref name="max" />.
        /// </summary>
        /// <typeparam name="T">The type of the value to clamp. Must implement <see cref="IComparable{T}" />.</typeparam>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value to clamp to.</param>
        /// <param name="max">The maximum value to clamp to.</param>
        /// <returns>The clamped value.</returns>
        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            T result = value;
            if (value.CompareTo(min) < 0)
                result = min;
            if (value.CompareTo(max) > 0)
                result = max;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="nextItem"></param>
        /// <param name="canContinue"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem, Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
                yield return current;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="nextItem"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem) where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }

        /// <summary>
        /// Converts a string to <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="value">The string to convert.</param>
        /// <returns>The converted value.</returns>
        public static T Get<T>(string value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));

            return (T)converter.ConvertFromString(value);
        }

        #endregion

        #region Messenger Helpers

        /// <summary>
        /// Used with relay commands to message recipients.
        /// </summary>
        /// <param name="notification">A string containing any arbitrary message to be passed to recipients.</param>
        public static void SendMessage(string notification)
        {
            SendMessage(null, notification);
        }

        /// <summary>
        /// Used with relay commands to message recipients.
        /// </summary>
        /// <param name="sender">The message's sender.</param>
        /// <param name="notification">A string containing any arbitrary message to be passed to recipients.</param>
        public static void SendMessage(object sender, string notification)
        {
            SendMessage(sender, null, notification);
        }

        /// <summary>
        /// Used with relay commands to message recipients.
        /// </summary>
        /// <param name="sender">The message's sender.</param>
        /// <param name="target">
        /// The message's intended target. This parameter can be used to give an indication 
        /// as to whom the message was intended for. Of course this is only an indication, 
        /// and may be null.
        /// </param>
        /// <param name="notification">A string containing any arbitrary message to be passed to recipients.</param>
        public static void SendMessage(object sender, object target, string notification)
        {
            var message = new NotificationMessage(sender, target, notification);
            Messenger.Default.Send(message);
        }

        /// <summary>
        /// Passes a string message (Notification) and a generic value (Content) to a recipient.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="content" />.</typeparam>
        /// <param name="content">A value to be passed to recipients.</param>
        /// <param name="notification">A string containing any arbitrary message to be passed to recipients.</param>
        public static void SendMessage<T>(T content, string notification)
        {
            SendMessage<T>(null, content, notification);
        }

        /// <summary>
        /// Passes a string message (Notification) and a generic value (Content) to a recipient.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="content" />.</typeparam>
        /// <param name="sender">The message's sender.</param>
        /// <param name="content">A value to be passed to recipients.</param>
        /// <param name="notification">A string containing any arbitrary message to be passed to recipients.</param>
        public static void SendMessage<T>(object sender, T content, string notification)
        {
            SendMessage<T>(sender, null, content, notification);
        }

        /// <summary>
        /// Passes a string message (Notification) and a generic value (Content) to a recipient.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="content" />.</typeparam>
        /// <param name="sender">The message's sender.</param>
        /// <param name="target">
        /// The message's intended target. This parameter can be used to give an indication 
        /// as to whom the message was intended for. Of course this is only an indication, 
        /// and may be null.
        /// </param>
        /// <param name="content">A value to be passed to recipients.</param>
        /// <param name="notification">A string containing any arbitrary message to be passed to recipients.</param>
        public static void SendMessage<T>(object sender, object target, T content, string notification)
        {
            var message = new NotificationMessage<T>(sender, target, content, notification);
            Messenger.Default.Send<NotificationMessage<T>>(message);
        }

        /// <summary>
        /// Provides a message class with a built-in callback. When the recipient is done 
        /// processing the message, it can execute the callback to notify the sender that it
        /// is done.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the callback.</typeparam>
        /// <param name="notification">A string containing any arbitrary message to be passed to recipients.</param>
        /// <param name="callback">The callback that may be executed by the receiver of the message.</param>
        public static void SendMessage<T>(string notification, Action<T> callback)
        {
            SendMessage<T>(null, notification, callback);
        }

        /// <summary>
        /// Provides a message class with a built-in callback. When the recipient is done 
        /// processing the message, it can execute the callback to notify the sender that it
        /// is done.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the callback.</typeparam>
        /// <param name="sender">The message's sender.</param>
        /// <param name="notification">A string containing any arbitrary message to be passed to recipients.</param>
        /// <param name="callback">The callback that may be executed by the receiver of the message.</param>
        public static void SendMessage<T>(object sender, string notification, Action<T> callback)
        {
            SendMessage<T>(sender, null, notification, callback);
        }

        /// <summary>
        /// Provides a message class with a built-in callback. When the recipient is done 
        /// processing the message, it can execute the callback to notify the sender that it
        /// is done.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the callback.</typeparam>
        /// <param name="sender">The message's sender.</param>
        /// <param name="target">
        /// The message's intended target. This parameter can be used to give an indication 
        /// as to whom the message was intended for. Of course this is only an indication, 
        /// and may be null.
        /// </param>
        /// <param name="notification">A string containing any arbitrary message to be passed to recipients.</param>
        /// <param name="callback">The callback that may be executed by the receiver of the message.</param>
        public static void SendMessage<T>(object sender, object target, string notification, Action<T> callback)
        {
            var message = new NotificationMessageAction<T>(sender, target, notification, callback);
            Messenger.Default.Send(message);
        }

        #endregion

        #region FormattedText Helpers

        /// <summary>
        /// Creates a <see cref="FormattedText" /> object using the specified text and default values.
        /// </summary>
        /// <param name="textToFormat">The text to format.</param>
        /// <returns>The formatted text.</returns>
        public static FormattedText CreateFormattedText(string textToFormat)
        {
            return CreateFormattedText(textToFormat, null);
        }

        /// <summary>
        /// Creates a <see cref="FormattedText" /> object using the specified text and default values.
        /// </summary>
        /// <param name="textToFormat">The text to format.</param>
        /// <param name="typeface">The type face to format the text with.</param>
        /// <returns>The formatted text.</returns>
        public static FormattedText CreateFormattedText(string textToFormat, Typeface typeface)
        {
            return CreateFormattedText(textToFormat, typeface, 0);
        }

        /// <summary>
        /// Creates a <see cref="FormattedText" /> object using the specified text and default values.
        /// </summary>
        /// <param name="textToFormat">The text to format.</param>
        /// <param name="typeface">The type face to format the text with.</param>
        /// <param name="emSize">The font size.</param>
        /// <returns>The formatted text.</returns>
        public static FormattedText CreateFormattedText(string textToFormat, Typeface typeface, double emSize)
        {
            return CreateFormattedText(textToFormat, typeface, emSize, null);
        }

        /// <summary>
        /// Creates a <see cref="FormattedText" /> object using the specified text and default values.
        /// </summary>
        /// <param name="textToFormat">The text to format.</param>
        /// <param name="typeface">The type face to format the text with.</param>
        /// <param name="emSize">The font size.</param>
        /// <param name="foreground">The color of the text.</param>
        /// <returns>The formatted text.</returns>
        public static FormattedText CreateFormattedText(string textToFormat, Typeface typeface, double emSize, Brush foreground)
        {
            if (typeface == null)
                typeface = new Typeface("Consolas");

            if (emSize == 0)
                emSize = 12;

            if (foreground == null)
                foreground = Brushes.Black;

            return new FormattedText(textToFormat, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, typeface, emSize, foreground);
        }

        /// <summary>
        /// Gets the size of a <see cref="FormattedText" /> object.
        /// </summary>
        /// <param name="formattedText">The formatted text.</param>
        /// <returns>The size of the formatted text.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="formattedText" /> cannot be null.</exception>
        public static Size GetSize(this FormattedText formattedText)
        {
            if (formattedText == null)
                throw new ArgumentNullException("formattedText");

            return new Size(formattedText.Width, formattedText.Height);
        }

        #endregion

        #endregion
    }
}
