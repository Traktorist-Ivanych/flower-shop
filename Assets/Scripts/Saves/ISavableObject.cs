namespace Saves
{
    public interface ISavableObject
    {
        public string PlayerPrefsKey { get; set; }
        
        public void Save()
        {
            
        }

        public void Load()
        {
            
        }
    }
}