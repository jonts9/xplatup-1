using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Xamarin.Forms;
using XplatCollect.Models;
using XplatCollect.Services;
using XplatCollect.ViewModels;
using XplatCollect.Views;

namespace XplatCollect
{
    public partial class App : PrismApplication
    {
        public App() : this(null)
        {
        }

        public App(IPlatformInitializer platformInitializer)
            : base(platformInitializer, true)
        {
        }

        public App(IPlatformInitializer platformInitializer, bool setFormsDependencyResolver)
            : base(platformInitializer, setFormsDependencyResolver)
        {
        }


        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<HomePage>();
            containerRegistry.RegisterForNavigation<ProfilePage>();
            containerRegistry.RegisterForNavigation<NewCollectionPage>();
            containerRegistry.RegisterForNavigation<CollectionPage>();
            containerRegistry.RegisterForNavigation<NewProfilePage>();

            containerRegistry.Register<IDataBaseService<Collection>, DataBaseService<Collection>>();
            containerRegistry.Register<IDataBaseService<Person>, DataBaseService<Person>>();
            containerRegistry.Register<IPersonService, PersonService>();
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService
                .NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainPage)}?selectedTab={nameof(HomePage)}");
        }
    }
}
