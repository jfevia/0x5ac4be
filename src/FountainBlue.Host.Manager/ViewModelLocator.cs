using CommonServiceLocator;
using FountainBlue.Host.Manager.Main;
using GalaSoft.MvvmLight.Ioc;

namespace FountainBlue.Host.Manager
{
    internal class ViewModelLocator
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ViewModelLocator" /> class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainWindowViewModel>();
        }

        public MainWindowViewModel MainWindow => ServiceLocator.Current.GetInstance<MainWindowViewModel>();
    }
}