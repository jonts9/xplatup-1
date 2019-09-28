using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using XplatCollect.Models;
using XplatCollect.Services;
using XplatCollect.Views;

namespace XplatCollect.ViewModels
{
    public sealed class NewProfilePageViewModel : ViewModelBase
    {
        private readonly IPersonService personService;

        public NewProfilePageViewModel(INavigationService navigationService
            , IPageDialogService pageDialogService
            , IPersonService personService)
            : base(navigationService, pageDialogService)
        {
            this.personService = personService;

            SaveProfileCommand = new DelegateCommand(async () => ExecuteSaveProfile())
                .ObservesCanExecute(() => IsNotBusy);

            TakePictureCommand = new DelegateCommand(async () => await ExecuteTakePicture())
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

        public ICommand SaveProfileCommand { get; }
        public ICommand TakePictureCommand { get; }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            return base.InitializeAsync(parameters);
        }

        protected override Task OnTabActive()
        {
            return base.OnTabActive();
        }

        private async Task ExecuteSaveProfile()
        {
            //TODO navegar para a página de cadastro, criar uma nova pessoa e retornar para tela de Profile.
            // Explorar, navigation service \ go back
            // Explorar inclusão de informações


            await ExecuteBusyAction(async () =>
            {
                var person = Person.Create(Name, Bio, Avatar);

                personService.Add(person);
            });

            await navigationService.NavigateAsync($"{nameof(ProfilePage)}");
        }

        private async Task ExecuteTakePicture()
        {
            try
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await this.pageDialogService.DisplayAlertAsync("No Camera", ":( No camera available.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Directory = AppConstants.IMAGE_PATH,
                    SaveToAlbum = true,
                    CompressionQuality = 75,
                    CustomPhotoSize = 50,
                    PhotoSize = PhotoSize.Small,
                    MaxWidthHeight = 420,
                    DefaultCamera = CameraDevice.Front
                });

                if (file == null)
                {
                    return;
                }

                Avatar = file.Path;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
