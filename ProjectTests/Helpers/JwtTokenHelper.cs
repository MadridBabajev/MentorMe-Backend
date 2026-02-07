using System.Text;
using Newtonsoft.Json;

namespace ProjectTests.Helpers;

public static class JwtTokenHelper
{
    public static Guid GetUserIdFromToken(string token)
    {
        // The token is in the format Header.Payload.Signature, and Payload is the part that contains the data
        var payloadPart = token.Split('.')[1];

        // The payload part is base64 encoded, so we need to decode it
        var base64PayloadPart = payloadPart.PadRight(payloadPart.Length + (4 - payloadPart.Length % 4) % 4, '=');
        var payload = Encoding.UTF8.GetString(Convert.FromBase64String(base64PayloadPart));

        // The payload is JSON encoded, so we can deserialize it
        var jwtPayload = JsonConvert.DeserializeObject<Dictionary<string, object>>(payload);

        // Extract and return the UserId
        if (jwtPayload!.ContainsKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))
        {
            return Guid.Parse(jwtPayload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"].ToString()!);
        }
        throw new ArgumentException("The token does not contain a UserId");
    }
}