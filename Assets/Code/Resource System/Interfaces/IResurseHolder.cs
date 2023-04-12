namespace ResourceSystem
{     
    public enum ResourceType
    {
        None,
        Wood,
        Iron,
        Deer,
        Horse,
        MagikStones,
        Textile,
        Steele,
        Gold,
    }
    
    public interface IResurseHolder:IHolder<ResurseCraft>
    {               
        
    }
}
