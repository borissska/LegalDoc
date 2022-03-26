using CommonServiceLocator;
using LegalDoc.Models;
using SolrNet;
using System;

namespace LegalDoc.CRUDSolr
{
    public class Operations
    {
        ISolrOperations<Document> solr = ServiceLocator.Current.GetInstance<ISolrOperations<Document>>();

        public string ClearSentence(string sentence)
        {
            char[] separators = { ' ', '.', ',', ';', ':', '"', '(', ')', '[', ']', '!', '?', '-', '_', '+', '=' };
            string[] stopWords = { "или", "но", "дабы", "затем", "потом", "лишь", "только", "он", "она", "оно", "мы", "его", "ее", "её", "вы", "вам", "вас", "что", "то",
            "который", "которая", "которые", "которых","их", "все", "всех", "всё","они", "я", "весь", "вся", "всю", "мне", "нам", "им", "меня", "нас", "таким", "таких",
            "такие", "те", "для", "так", "на", "по", "со", "из", "от", "до", "без", "над", "под", "за", "при", "после", "во", "не", "же", "то", "бы", "всего", "итого",
            "даже", "да", "нет", "ой", "ого", "эх", "браво", "здравствуйте", "спасибо", "извините", "скажем", "допустим", "например", "самом", "однако", "вообще", "общем", "вероятно",
            "й", "ц", "у", "к", "е", "н", "г", "ш", "щ", "з", "х", "ъ", "ф", "ы", "в", "а", "п", "р", "о", "л", "д", "ж", "э", "я",
            "я", "ч", "с", "м", "и", "т", "ь", "б", "ю", "дальше", "ближе" };

            string newSentence = "";
            string lowerSentence = sentence.ToLower();
            string[] sentenceArr = lowerSentence.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            for (int iter = 0; iter < sentenceArr.Length; iter++)
                foreach (string itemStop in stopWords)
                    if (sentenceArr[iter] == itemStop)
                        for (int i = iter - 1; i < sentenceArr.Length - 1; i++)
                            sentenceArr[i] = sentenceArr[i + 1];

            for (int iter = 0; iter < sentenceArr.Length; iter++)
                newSentence += sentenceArr[iter] + " ";

            return newSentence.Remove(newSentence.Length - 1, 1);
        }

        public void Create(Document document)
        {
            Document result = new Document();
            
            result.Id = document.Id;
            result.Title = ClearSentence(document.Title);
            result.DocText = ClearSentence(document.DocText);
            result.Author = ClearSentence(document.Author);
            result.AcceptTime = document.AcceptTime;

            solr.Add(result);
            solr.Commit();
        }

        public Document Read(int Id)           
        {
            var result = solr.Query(new SolrQueryByField("id", Id.ToString()));
            return result[0];
        }

        public List<int> Find(string Input)
        {
            List<int> result = new List<int>();
            char[] separators = { ' ' };
            string[] MassivSlov = Input.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            foreach (string slovo in MassivSlov)
            {
                var result_title = solr.Query(new SolrQueryByField("doctitle", slovo));
                
                foreach (Document document in result_title)
                {
                    result.Add(document.Id);
                }
            }

            foreach (string slovo in MassivSlov)
            {
                SolrQueryResults<Document> result_text = solr.Query(new SolrQueryByField("doctext", slovo));

                foreach (Document document in result_text)
                {
                    if (result.Count > 0)
                    {
                        foreach (int docID in result)
                            if (docID != document.Id)
                                result.Add(document.Id);
                    }
                    else
                        result.Add(document.Id);
                }
            }

            foreach (string slovo in MassivSlov)
            {
                SolrQueryResults<Document> result_author = solr.Query(new SolrQueryByField("author", slovo));
                int i = 1;
                foreach (Document document in result_author)
                {
                    if (result.Count > 0)
                    {
                        foreach (int docID in result)
                            if (docID != document.Id)
                                result.Add(document.Id);
                    }
                    else
                        result.Add(document.Id);
                }
            }

            if (result.Count == 0)
                    return null;
                else
                    return result;
        }

        public void Update(int Id, Document document)
        {
            solr.Delete(new SolrQueryByField("id", Id.ToString()));
            solr.Add(document);
            solr.Commit();
        }

        public void Delete(int Id)
        {
            solr.Delete(new SolrQueryByField("id", Id.ToString()));
            solr.Commit();
        }
    }
}
