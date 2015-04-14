using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SACAAE
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IServiceMobile" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IServiceMobile
    {
        [OperationContract]
        string DoWork();

        [OperationContract]
        string HelloWorld();

        [OperationContract]
        string Login(String pMail);
        [OperationContract]
        string GetProyectos(String pMail);
        [OperationContract]
        string GetComisiones(String pMail);
        [OperationContract]
        string GetCursos(String pMail);
    }
}
