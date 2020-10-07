using Autofac;

namespace KataTrainer {
    public class KataTrainerModule : Module {

        public KataTrainerModule() {
        }

        protected sealed override void Load(ContainerBuilder builder) {

            //resolver all stuffs here

            base.Load(builder);
        }
    }
}
