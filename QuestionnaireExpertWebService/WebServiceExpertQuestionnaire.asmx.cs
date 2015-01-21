using QuestionnaireWebExpert.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Services;

namespace QuestionnaireExpertWebService
{
    /// <summary>
    /// Description résumée de WebServiceExpertQuestionnaire
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Pour autoriser l'appel de ce service Web depuis un script à l'aide d'ASP.NET AJAX, supprimez les marques de commentaire de la ligne suivante. 
    // [System.Web.Script.Services.ScriptService]
    public class WebServiceExpertQuestionnaire : System.Web.Services.WebService
    {
        static IDictionary<string, Question> _mesQuestionsTraitees;
        static IDictionary<string, Question> _mesQuestionsNonTraitees;
        static IDictionary<string, Reponse> _mesReponses;
        
        [WebMethod]
        public string PoserQuestion(string uneQuestion)
        {
            string reponse = "";

            if (AjouterQuestion(uneQuestion))
            {
                if (String.IsNullOrEmpty(GetQuestion(ConvertirReponseEnIdReponseOuQuestion(uneQuestion)).GetReponse().ReponseString))
                {
                    HttpContext.Current.Response.AppendHeader("Content-Type", "text/html; charset=utf-8");
                    reponse = string.Format("<hml><head><title>Question</title><form action='http://localhost:1911/WebServiceExpertQuestionnaire.asmx?op=GetQuestion' method='post'><p>Question : {0}</p><p>Réponse : <emb>(Pas de réponse disponible pour le moment) vous consulter la réponse sur <a href='http://localhost:1911/WebServiceExpertQuestionnaire.asmx?op=GetQuestion'>http://localhost:1911/WebServiceExpertQuestionnaire.asmx?op=GetQuestion</a> et saisir l'identifiant : {1}</emb></p></form></a></body></html>", uneQuestion, GetQuestion(ConvertirReponseEnIdReponseOuQuestion(uneQuestion)).Id);
                }
                else
                {
                    HttpContext.Current.Response.AppendHeader("Content-Type", "text/html; charset=utf-8");
                    reponse = string.Format("<hml><head><title>Question</title><form action='http://localhost:1911/WebServiceExpertQuestionnaire.asmx?op=GetQuestion' method='post'><p>Question : {0}</p><p>Réponse : <emb>{1}</emb></p></form></a></body></html>", uneQuestion, GetQuestion(ConvertirReponseEnIdReponseOuQuestion(uneQuestion)).GetReponse().ReponseString);
                }
            }

            return reponse;
        }

        [WebMethod]
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
                    _mesQuestionsNonTraitees.Add(GetIdReponseSucsception(questionPose), new Question(questionPose));
                    ajoutReussi = true;
                }
            }

            return ajoutReussi;
        }

        [WebMethod]
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

        [WebMethod]
        public Question GetQuestion(string idQuestion)
        {
            Question uneQuestion = new Question();

            if (_mesQuestionsTraitees.ContainsKey(idQuestion))
            {
                uneQuestion = _mesQuestionsTraitees[idQuestion];
            }

            if (_mesQuestionsNonTraitees.ContainsKey(idQuestion))
            {
                uneQuestion = _mesQuestionsNonTraitees[idQuestion];
            }

            return uneQuestion;
        }

        [WebMethod]
        public Reponse GetReponse(string idReponse)
        {
            Reponse uneReponse = new Reponse();

            if (_mesReponses.ContainsKey(idReponse))
            {
                uneReponse = _mesReponses[idReponse];
            }

            return uneReponse;
        }

        [WebMethod]
        public string GetIdReponseSucsception(string reponse)
        {
            return ConvertirReponseEnIdReponseOuQuestion(reponse);
        }

        [WebMethod]
        public string ConvertirReponseEnIdReponseOuQuestion(string reponseOuQuestion)
        {
            return reponseOuQuestion.ToUpperInvariant();
        }

        [WebMethod]
        public void InitialiserGestionnaire()
        {
            _mesQuestionsNonTraitees = new Dictionary<string, Question>();
            _mesQuestionsTraitees = new Dictionary<string, Question>();
            _mesReponses = new Dictionary<string, Reponse>();
        }

        [WebMethod]
        IEnumerable<Question> Questions()
        {
            return _mesQuestionsNonTraitees.Values;
        }

        [WebMethod]
        IEnumerable<Reponse> Reponses()
        {
            return _mesReponses.Values;
        }
    }

    [DataContract]
    public class HtmlObjet : HtmlString
    {
        public HtmlObjet()
            : base("")
        {
        }

        public HtmlObjet(string value)
            : base(value)
        {
        }
    }
}
