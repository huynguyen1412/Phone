
namespace ICommandSample {
    public class Model {
        private string _data;
        private bool _dataDirty;

        public Model() { 
            _data = null;
            _dataDirty = false;
        }

        public bool DataDirty {
            get {
                return _dataDirty;
            }

            set {
                _dataDirty = value;
                // Notify the model as necessary
            }
        }
        public object Data {
            get {
                return _data; 
            }

            set {
                _data = value as string;
                // Notify the model as necessary
            }
        }
    }
}
