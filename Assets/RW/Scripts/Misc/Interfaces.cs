//Character Types use this interface to derive the Damage function
//so that the damage system can be expanded upon if more character
//types are added in future iterations
public interface IDamageable 
{
    void Damage();
}