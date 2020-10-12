using System.Collections.Generic;
using KataMvvMModule;

namespace TypingKata {

    /// <summary>
    /// The root view model and subsequent view is what will encompass the whole program.
    /// By keeping track of all the views and viewmodels I can easily edit and remove them on the fly at runtime.
    /// </summary>
    public class RootModel : IModel {

        private readonly IDictionary<IView, IViewModel<IModel>> _viewModels;

        public IDictionary<IView, IViewModel<IModel>> ViewModels => _viewModels;

        public RootModel() {
            _viewModels = new Dictionary<IView, IViewModel<IModel>>();
        }

        /// <summary>
        /// Saves view and viewmodel to the ViewModels dictionary and maps them to each other with data context.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="view"></param>
        public void AddNewViewModel(IViewModel<IModel> viewModel, IView view) {
            if (viewModel != null && view != null) {
                view.DataContext = viewModel;
                _viewModels.Add(new KeyValuePair<IView, IViewModel<IModel>>(view, viewModel));
            }
        }
    }
}