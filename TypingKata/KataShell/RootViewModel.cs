using GalaSoft.MvvmLight;
using KataIocModule;

namespace TypingKata {
    public class RootViewModel : ViewModelBase, IRootViewModel {
        private IContainerBuilderFacade _builder;

        public RootViewModel(IContainerBuilderFacade builder) {
            _builder = builder;
        }
    }
}