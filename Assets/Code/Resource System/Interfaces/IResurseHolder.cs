namespace ResourceSystem
{     
    public enum ResourceType
    {
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
