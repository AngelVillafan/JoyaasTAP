using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using JoyaasTAP.Models;
using Newtonsoft.Json;

namespace JoyaasTAP.ViewModels
{
    public class JoyasViewModel: INotifyPropertyChanged
    {

        //Propiedad para manejar las joyas
        private Joya? joya;

        public Joya? LaJoya
        {
            get { return joya; }
            set { joya = value;
                PropertyChange();
            }
        }

        //Lista donde se almacenaran las joyas
        public ObservableCollection<Joya> Joyeria { get; set; } = new ObservableCollection<Joya>();

        //Comandos para ejecutar los metodos
        public ICommand AgregarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand CambiarVistaCommand { get; set; }

        //Propiedad Vista, para cambiar entre vistas
        public string Vista { get; set; } = "Ver";
        //Propiedad Error, aqui se nos mostraran los errores
        public string Error { get; set; } = "";


        //Constructor de nuestro ViewModel
        public JoyasViewModel()
        {       
            AgregarCommand = new RelayCommand(Agregar);
            CancelarCommand = new RelayCommand(Cancelar);
            EliminarCommand = new RelayCommand(Eliminar);
            CambiarVistaCommand = new RelayCommand<string>(CambiarVista);
            Joyeria = new ObservableCollection<Joya>();
            Cargar();

        }



        // METODOS

        //Metodo para eliminar alguna joya
        private void Eliminar()
        {
            try
            {
                if (LaJoya != null)
                {
                    Joyeria.Remove(LaJoya);
                    Guardar();
                    PropertyChange();
                }
            }
            catch (Exception )
            {
                Error = "Si desea eliminar un elemento, primero seleccionelo de la lista.";
            }
        }


        //Metodo para cancelar el agregado de una joya
        private void Cancelar()
        {
            try
            {
                LaJoya = null;
                CambiarVista("Ver");
            }
            catch(Exception ex)
            {
                Error=ex.Message;
            }
        }

        //Metodo para agregar joyas a nuesrtra joyeria
        private void Agregar()
        {
            try
            {
                if (LaJoya != null)
                {
                    // Hacer validación
                    if (string.IsNullOrWhiteSpace(LaJoya.Nombre))
                    {
                        Error = "Proporcione el nombre de su articulo.";
                        PropertyChange("Error");
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(LaJoya.Tipo.ToString()))
                    {
                        Error = "Seleccione una categoria del producto.";
                        PropertyChange("Error");
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(LaJoya.Origen))
                    {
                        Error = "¿De donde proviene su joya?, Por favor ingrese el dato.";
                        PropertyChange("Error");
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(LaJoya.Peso))
                    {
                        Error = "¿De cuantos gramos es su piedra? Por favor indiquelo en peso.";
                        PropertyChange("Error");
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(LaJoya.Descripcion))
                    {
                        Error = "Por favor ingrese algo de informacion de su pieza en la descripcion.";
                        PropertyChange("Error");
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(LaJoya.Foto))
                    {
                        Error = "Teclee una URL para la foto de la pieza que gusta registrar.";
                        PropertyChange("Error");
                        return;
                    }
                    Uri? uri;
                    if (!Uri.TryCreate(LaJoya.Foto, UriKind.Absolute, out uri))
                    {
                        Error = "Teclee una URL válida para la foto de su pieza a registrar.";
                        PropertyChange("Error");
                        return;
                    }
                    Joyeria.Add(LaJoya);
                    Guardar();
                    Vista = "Ver";
                    PropertyChange();
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            
        }

        private void CambiarVista(string vista)
        {
            try
            {
                Vista = vista;
                if (vista == "Agregar")
                {
                    LaJoya = new Joya();
                }              
            }
            catch(Exception ex)
            {
                Error = ex.Message;
            }
            
        }


        //PERSISTENCIA DE DATOS

        //Desserializacion de la informacion que este guardada
        private void Cargar()
        {
            if (File.Exists("joyas.json"))
            {
                var json = File.ReadAllText("joyas.json");
                var datos = JsonConvert.DeserializeObject<ObservableCollection<Joya>>(json);
                if (datos != null)
                {
                    Joyeria = datos;
                }
                else
                {
                    Joyeria = new ObservableCollection<Joya>();
                }
                PropertyChange();
            }
        }

        //Serializacion de la informacion que este guardada
        private void Guardar()
        {
            if (LaJoya != null)
            {
                File.WriteAllText("joyas.json", JsonConvert.SerializeObject(Joyeria));
            }
        }

        //Metodo para actualizar las propiedades
        void PropertyChange(string? propiedad = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propiedad));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
