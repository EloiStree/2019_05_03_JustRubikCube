using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GitHub
{

    public class NotImplementedIssueException : System.NotImplementedException
    {

        public string m_issueId;
        public string m_githubProjectUrl;

        public NotImplementedIssueException(string id, string projectUrl="")
        {


        }
    }
    public class NotImplementedIssueByKeywordException : System.NotImplementedException
    {

        public string[] keywords;
        public string m_githubProjectUrl;

        public NotImplementedIssueByKeywordException(string []keywords, string projectUrl = "")
        {

            if (string.IsNullOrEmpty(projectUrl))
                projectUrl = TryToAccessUserPreference();
        }

        private string TryToAccessUserPreference()
        {
            throw new System.NotImplementedException("Need to code that when I have internet to look for 'Find all files with githubissuetracker.json'");
        }
    }
}
