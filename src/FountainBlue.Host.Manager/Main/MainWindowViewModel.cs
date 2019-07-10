using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FountainBlue.Host.Manager.Properties;
using FountainBlue.Service.Core;
using FountainBlue.Service.Provider;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using log4net;

namespace FountainBlue.Host.Manager.Main
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly ContractCallback _contractCallback;
        private readonly WSDualHttpBinding _dualHttpBinding;
        private readonly EndpointAddress _endpointAddress;
        private readonly InstanceContext _instanceContext;
        private readonly ILog _log;
        private readonly DataServiceClient _serviceClient;
        private ObservableCollection<ClientEndpoint> _clients;
        private ObservableCollection<string> _consoleOutput;
        private ICommand _executeCommand;
        private bool _isBusy;
        private ICommand _refreshCommand;
        private ObservableCollection<ScriptItem> _scripts;
        private ClientEndpoint _selectedClient;
        private ObservableCollection<ScriptItem> _selectedScripts;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindowViewModel" /> class.
        /// </summary>
        public MainWindowViewModel()
        {
            ConsoleOutput = new ObservableCollection<string>();

            MessengerInstance.Register<ScriptsSelectionChangedMessage>(this, OnScriptsSelectionChanged);

            _log = LogManager.GetLogger(GetType());

            _contractCallback = new ContractCallback();
            _contractCallback.LoadingScripts += OnLoadingScripts;
            _contractCallback.LoadingClients += OnLoadingClients;
            _contractCallback.ReportingStatus += OnReportingStatus;
            _instanceContext = new InstanceContext(_contractCallback);
            _dualHttpBinding = new WSDualHttpBinding(WSDualHttpSecurityMode.None);
            _endpointAddress = new EndpointAddress(Settings.Default.ServiceAddress);
            _serviceClient = new DataServiceClient(_instanceContext, _dualHttpBinding, _endpointAddress);

            _serviceClient.Connect();
        }

        /// <summary>
        ///     Gets or sets the console output.
        /// </summary>
        /// <value>
        ///     The console output.
        /// </value>
        public ObservableCollection<string> ConsoleOutput
        {
            get => _consoleOutput;
            set => Set(ref _consoleOutput, value);
        }

        /// <summary>
        ///     Gets or sets the selected scripts.
        /// </summary>
        /// <value>
        ///     The selected scripts.
        /// </value>
        public ObservableCollection<ScriptItem> SelectedScripts
        {
            get => _selectedScripts;
            set => Set(ref _selectedScripts, value);
        }

        /// <summary>
        ///     Gets or sets the clients.
        /// </summary>
        /// <value>
        ///     The clients.
        /// </value>
        public ObservableCollection<ClientEndpoint> Clients
        {
            get => _clients;
            set => Set(ref _clients, value);
        }

        /// <summary>
        ///     Gets or sets the scripts.
        /// </summary>
        /// <value>
        ///     The scripts.
        /// </value>
        public ObservableCollection<ScriptItem> Scripts
        {
            get => _scripts;
            set => Set(ref _scripts, value);
        }

        /// <summary>
        ///     Gets or sets the selected client.
        /// </summary>
        /// <value>
        ///     The selected client.
        /// </value>
        public ClientEndpoint SelectedClient
        {
            get => _selectedClient;
            set => Set(ref _selectedClient, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is busy.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is busy; otherwise, <c>false</c>.
        /// </value>
        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }

        /// <summary>
        ///     Gets the execute command.
        /// </summary>
        /// <value>
        ///     The execute command.
        /// </value>
        public ICommand ExecuteCommand => _executeCommand ?? (_executeCommand = new RelayCommand(ExecuteScript, () => !IsBusy && SelectedScripts != null && SelectedScripts.Any() && SelectedClient != null));

        /// <summary>
        ///     Gets the refresh command.
        /// </summary>
        /// <value>
        ///     The refresh command.
        /// </value>
        public ICommand RefreshCommand => _refreshCommand ?? (_refreshCommand = new RelayCommand(Refresh, () => !IsBusy));

        /// <summary>
        ///     Called when [reporting status].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusEventArgs" /> instance containing the event data.</param>
        private void OnReportingStatus(object sender, StatusEventArgs e)
        {
            var message = $"{DateTime.Now} >> [{e.Status.Client.Address}:{e.Status.Client.Port}] {e.Status.Message}";
            ConsoleOutput.Add(message);
        }

        /// <summary>
        ///     Called when [scripts selection changed].
        /// </summary>
        /// <param name="obj">The object.</param>
        private void OnScriptsSelectionChanged(ScriptsSelectionChangedMessage obj)
        {
            SelectedScripts = new ObservableCollection<ScriptItem>(obj.Scripts);
        }

        /// <summary>
        ///     Called when [loading clients].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ClientsEventArgs" /> instance containing the event data.</param>
        private void OnLoadingClients(object sender, ClientsEventArgs e)
        {
            try
            {
                Clients = new ObservableCollection<ClientEndpoint>(e.Clients);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        /// <summary>
        ///     Refreshes the list of available scripts.
        /// </summary>
        private async void Refresh()
        {
            try
            {
                IsBusy = true;
                await Task.Run(() =>
                {
                    _serviceClient.LoadScripts();
                    _serviceClient.LoadClients();
                });
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        ///     Called when [loading scripts].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ScriptItemsEventArgs" /> instance containing the event data.</param>
        private void OnLoadingScripts(object sender, ScriptItemsEventArgs e)
        {
            try
            {
                Scripts = new ObservableCollection<ScriptItem>(e.Scripts);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        /// <summary>
        ///     Executes the selected script.
        /// </summary>
        private async void ExecuteScript()
        {
            try
            {
                IsBusy = true;
                await Task.Run(() =>
                {
                    foreach (var selectedScript in _selectedScripts)
                        _serviceClient.ExecuteScript(_selectedClient, selectedScript.Id);
                });
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}