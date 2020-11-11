using GalaSoft.MvvmLight;

namespace TypingKata {
    public class RootViewModel : ViewModelBase, IRootViewModel {
        private IContainerBuilderFacade _builder;

        public RootViewModel(IContainerBuilderFacade builder) {
            this._builder = builder;
        }
    }
}