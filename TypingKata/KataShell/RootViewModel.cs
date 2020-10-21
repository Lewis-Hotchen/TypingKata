using GalaSoft.MvvmLight;

namespace TypingKata {
    public class RootViewModel : ViewModelBase, IRootViewModel {


        public RootViewModel(IContainerBuilderFacade builderFacade) {
            builderFacade.ClearHistory();
        }
    }
}