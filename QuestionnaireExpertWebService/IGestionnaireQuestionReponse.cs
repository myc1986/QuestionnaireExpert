using QuestionnaireWebExpert.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace QuestionnaireExpertWebService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IGestionnaireQuestionReponse" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IGestionnaireQuestionReponse
    {
        [OperationContract]
        IEnumerable<Question> Questions();

        [OperationContract]
        IEnumerable<Reponse> Reponses();

        [OperationContract]
        bool AjouterQuestion(string questionPose);

        [OperationContract]
        bool AjouterReponse(string idQuestion, string reponse);

        [OperationContract]
        Question GetQuestion(string idQuestion);

        [OperationContract]
        Reponse GetReponse(string idReponse);

        [OperationContract]
        string GetIdReponseSucsception(string reponse);

        [OperationContract]
        string ConvertirReponseEnIdReponseOuQuestion(string reponseOuQuestion);

        [OperationContract]
        void InitialiserGestionnaire();
    }
}
