namespace wasty.Models
{
    /// <summary>
    /// Clase que representa la estructura estándar de las respuestas de la API.
    /// </summary>
    /// <typeparam name="T">El tipo de datos que se devuelve en la respuesta.</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indica si la solicitud fue exitosa.
        /// </summary>
        public bool Exito { get; set; }

        /// <summary>
        /// Mensaje asociado con la respuesta.
        /// </summary>
        public string Mensaje { get; set; }

        /// <summary>
        /// Datos devueltos en la respuesta.
        /// </summary>
        public T Datos { get; set; }

        /// <summary>
        /// Lista de errores, si los hay.
        /// </summary>
        public List<string> Errores { get; set; }

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public ApiResponse()
        {
            Errores = new List<string>();
        }

        /// <summary>
        /// Constructor con parámetros.
        /// </summary>
        /// <param name="exito">Indica si la solicitud fue exitosa.</param>
        /// <param name="mensaje">Mensaje asociado con la respuesta.</param>
        /// <param name="datos">Datos devueltos en la respuesta.</param>
        /// <param name="errores">Lista de errores, si los hay.</param>
        public ApiResponse(bool exito, string mensaje, T datos, List<string> errores = null)
        {
            Exito = exito;
            Mensaje = mensaje;
            Datos = datos;
            Errores = errores ?? new List<string>();
        }
    }
}