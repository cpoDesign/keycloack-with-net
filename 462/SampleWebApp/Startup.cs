using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Owin.Security.Keycloak;
using System;

[assembly: OwinStartup(typeof(SampleWebApp.Startup))]
namespace SampleWebApp
{
	public class Startup
	{
		const string persistentAuthType = "keycloak_cookies"; // Or name it whatever you want

		const string Relm = "demorelm";
		const string ClientId = "demo-app";
		const string ClientSecret = "8cb1af0c-1b47-4e7f-9be8-0950aeea5c71";
		const string KeyCloakUrl = "http://localhost:8080/auth/";

		public void Configuration(IAppBuilder app)
		{
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationType = persistentAuthType
			});

			// You may also use this method if you have multiple authentication methods below,
			// or if you just like it better:
			app.SetDefaultSignInAsAuthenticationType(persistentAuthType);

			app.UseKeycloakAuthentication(new KeycloakAuthenticationOptions
			{
				Realm = Relm,
				ClientId = ClientId,
				ClientSecret = ClientSecret,
				KeycloakUrl = KeyCloakUrl,
				//ResponseType = "id_token token",
				AuthenticationType = persistentAuthType,
				//AuthenticationMode = AuthenticationMode.Active,
				SignInAsAuthenticationType = persistentAuthType, // Not required with SetDefaultSignInAsAuthenticationType
																												 //Token validation options - these are all set to defaults
				AllowUnsignedTokens = false,
				DisableIssuerSigningKeyValidation = false,
				DisableIssuerValidation = false,
				DisableAudienceValidation = true,
				DisableRefreshTokenSignatureValidation = true,

				// DisableRefreshTokenSignatureValidation = true, // Fix for Keycloak server v4.5
				// DisableAllRefreshTokenValidation = true, // Fix for Keycloak server v4.6-4.8,  overrides DisableRefreshTokenSignatureValidation. The content of Refresh token was changed. Refresh token should not be used by the client application other than sending it to the Keycloak server to get a new Access token (where Keycloak server will validate it) - therefore validation in client application can be skipped.

				TokenClockSkew = TimeSpan.FromSeconds(2)
			});
		}
	}
}