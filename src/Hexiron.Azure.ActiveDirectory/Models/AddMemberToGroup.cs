namespace Hexiron.Azure.ActiveDirectory.Models
{
    public class AddMemberToGroup
    {
        private string _url;
        public AddMemberToGroup(string url)
        {
            _url = url;
        }
        public string Url
        {
            get
            {
                return _url;
            }
        }
    }
}

