namespace Public.DTO.v1.Identity;

/// <summary>
/// DTO that Specifies whether the user is trying to get the data of their own page or not
/// </summary>
public class ProfileDataRequest
{
    /// <summary>
    /// Id of the visited user, will be a null if the user is viewing their own page 
    /// </summary>
    public Guid? VisitedUserId { get; set; }
}
