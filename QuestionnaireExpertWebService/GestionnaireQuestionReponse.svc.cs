using QuestionnaireWebExpert.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace QuestionnaireExpertWebService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "GestionnaireQuestionReponse" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez GestionnaireQuestionReponse.svc ou GestionnaireQuestionReponse.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public class GestionnaireQuestionReponse : IGestionnaireQuestionReponse
    {
        private IDictionary<string, Question> _mesQuestionsTraitees = null;
        private IDictionary<string, Question> _mesQuestionsNonTraitees = null;
        private IDictionary<string, Reponse> _mesReponses = null;

        public GestionnaireQuestionReponse()
        {
            InitialiserGestionnaire();
        }

        public bool AjouterQuestion(string questionPose)
        {
            bool ajoutReussi = false;

            if (_mesQuestionsNonTraitees == null)
            {
                _mesQuestionsNonTraitees = new Dictionary<string, Question>();
            }

            if (_mesQuestionsTraitees == null)
            {
                _mesQuestionsTraitees = new Dictionary<string, Question>();
            }

            if (!_mesQuestionsTraitees.ContainsKey(GetIdReponseSucsception(questionPose)))
            {
                if (!_mesQuestionsNonTraitees.ContainsKey(GetIdReponseSucsception(questionPose)))
                {
                    _mesQuestionsNonTraitees.Add(questionPose, new Question(questionPose));
                    ajoutReussi = true;
                }
            }

            return ajoutReussi;
        }

        public bool AjouterReponse(string idQuestion, string reponse)
        {
            bool ajoutReussie = false;

            if (_mesReponses == null)
            {
                _mesReponses = new Dictionary<string, Reponse>();
            }

            if (_mesQuestionsNonTraitees.ContainsKey(idQuestion))
            {
                if (!_mesReponses.ContainsKey(GetIdReponseSucsception(reponse)))
                {
                    _mesQuestionsNonTraitees[idQuestion].Répondre(reponse);
                    _mesReponses.Add(_mesQuestionsNonTraitees[idQuestion].GetReponse().Id, _mesQuestionsNonTraitees[idQuestion].GetReponse().Clone());
                    _mesQuestionsTraitees.Add(_mesQuestionsNonTraitees[idQuestion].Id, _mesQuestionsNonTraitees[idQuestion].Clone());
                    _mesQuestionsNonTraitees.Remove(idQuestion);
                    ajoutReussie = true;
                }
            }

            return ajoutReussie;
        }

        public Question GetQuestion(string idQuestion)
        {
            Question uneQuestion = new Question();

            if (_mesQuestionsTraitees.ContainsKey(idQuestion))
            {
                uneQuestion = _mesQuestionsTraitees[idQuestion];
            }

            return uneQuestion;
        }

        public Reponse GetReponse(string idReponse)
        {
            Reponse uneReponse = new Reponse();

            if (_mesReponses.ContainsKey(idReponse))
            {
                uneReponse = _mesReponses[idReponse];
            }

            return uneReponse;
        }

        public string GetIdReponseSucsception(string reponse)
        {
            return ConvertirReponseEnIdReponseOuQuestion(reponse);
        }

        public string ConvertirReponseEnIdReponseOuQuestion(string reponseOuQuestion)
        {
            return reponseOuQuestion.ToUpperInvariant();
        }

        public void InitialiserGestionnaire()
        {
            _mesQuestionsNonTraitees = new Dictionary<string, Question>();
            _mesQuestionsTraitees = new Dictionary<string, Question>();
            _mesReponses = new Dictionary<string, Reponse>();
        }

        IEnumerable<Question> IGestionnaireQuestionReponse.Questions()
        {
            return _mesQuestionsNonTraitees.Values;
        }

        IEnumerable<Reponse> IGestionnaireQuestionReponse.Reponses()
        {
            return _mesReponses.Values;
        }
    }
}
