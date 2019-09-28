using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using XplatCollect.Models;
using XplatCollect.Services;
using XplatCollect.Views;

namespace XplatCollect.ViewModels
{
    public sealed class ProfilePageViewModel : ViewModelBase
    {
        private readonly IPersonService personService;

        public ProfilePageViewModel(INavigationService navigationService
            , IPageDialogService pageDialogService
            , IPersonService personService)
            : base(navigationService, pageDialogService)
        {
            this.personService = personService;

            CreateProfileCommand = new DelegateCommand(async () => ExecuteCreateProfile())
                .ObservesCanExecute(() => IsNotBusy);
        }

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string bio;
        public string Bio
        {
            get => bio;
            set => SetProperty(ref bio, value);
        }

        private string avatar;
        public string Avatar
        {
            get => avatar;
            set => SetProperty(ref avatar, value);
        }

        private int collectionsQnt;
        public int CollectionsQnt
        {
            get => collectionsQnt;
            set => SetProperty(ref collectionsQnt, value);
        }

        private int itensQnt;
        public int ItensQnt
        {
            get => itensQnt;
            set => SetProperty(ref itensQnt, value);
        }

        private bool hasProfile;
        public bool HasProfile
        {
            get => hasProfile;
            set => SetProperty(ref hasProfile, value);
        }

        public ICommand CreateProfileCommand { get; }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            LoadPerson();

            return base.InitializeAsync(parameters);
        }

        protected override Task OnTabActive()
        {
            LoadPerson();

            return base.OnTabActive();
        }

        private void LoadPerson()
        {
            HasProfile = personService.HasPerson();

            if (HasProfile)
            {
                var person = personService.GetPerson();

                Name = person.Name;
                Bio = person.Bio;
                Avatar = person.Avatar;
            }
        }

        private async Task ExecuteCreateProfile()
        {
            //TODO navegar para a página de cadastro, criar uma nova pessoa e retornar para tela de Profile.
            // Explorar, navigation service \ go back
            // Explorar inclusão de informações


            //await ExecuteBusyAction(async () =>
            //{
            //    var person = Person.Create("Zé das couves"
            //        , "Nascido nos Estados Unidos do Brasil e é uma das relíquias do Brasil.Ele encontrou a fonte da juventude passando pelo portal mágico da Pedreira Paulo Leminski", "https://www.mantelligence.com/wp-content/uploads/2017/10/Questions-To-Ask-Smile-or-eyes.jpg");

            //    personService.Add(person);

            //    LoadPerson();
            //});

            await navigationService.NavigateAsync($"{nameof(NewProfilePage)}");
        }
    }
}
