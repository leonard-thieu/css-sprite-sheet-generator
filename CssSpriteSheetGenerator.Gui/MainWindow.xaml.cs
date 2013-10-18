using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using CssSpriteSheetGenerator.Gui.Properties;
using CssSpriteSheetGenerator.Gui.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;

namespace CssSpriteSheetGenerator.Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Register<DialogMessage>(this,
                m =>
                {
                    var result = MessageBox.Show(this, m.Content, m.Caption, MessageBoxButton.YesNoCancel);
                    m.ProcessCallback(result);
                });

            Register<NotificationMessageAction<string>>(this,
                m =>
                {
                    switch (m.Notification)
                    {
                        case MainWindowViewModel.ExportNotification:
                            this.ExecuteDialog<SaveFileDialog>(".cssg", "CSS Sprite Sheet Generator Files|*.cssg|All Files|*.*", m.Execute);
                            break;
                        case MainWindowViewModel.ImportNotification:
                            this.ExecuteDialog<OpenFileDialog>(".cssg", "CSS Sprite Sheet Generator Files|*.cssg|All Files|*.*", m.Execute);
                            break;
                        case MainWindowViewModel.SaveImageNotification:
                            this.ExecuteDialog<SaveFileDialog>(".png", "Portable Network Graphics|*.png|All Files|*.*", m.Execute);
                            break;
                        case MainWindowViewModel.SaveCssNotification:
                            this.ExecuteDialog<SaveFileDialog>("*.css", "Cascading Style Sheets|*.css|All Files|*.*", m.Execute);
                            break;
                    }
                });

            Register<NotificationMessage>(this,
                m =>
                {
                    switch (m.Notification)
                    {
                        case MainWindowViewModel.ExitNotification:
                            Application.Current.Shutdown();
                            break;
                        case MainWindowViewModel.HelpNotification:
                            System.Windows.Forms.Help.ShowHelp(null, Settings.Default.HelpFile);
                            break;
                        case MainWindowViewModel.AboutNotification:
                            var aboutWindow = new AboutWindow { Owner = this };
                            aboutWindow.Show();
                            break;
                        case MainWindowViewModel.OptionsNotification:
                            break;
                    }
                });

            Register<NotificationMessage<string>>(this,
                m =>
                {
                    switch (m.Notification)
                    {
                        case "Exception":
                            MessageBox.Show(m.Content);
                            break;
                    }
                });
        }

        // Short-hand Register<T>() call
        private static void Register<TMessage>(object recipient, Action<TMessage> action)
        {
            Messenger.Default.Register<TMessage>(recipient, action);
        }
    }
}
