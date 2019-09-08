function fn() {
  // don't waste time waiting for a connection or if servers don't respond within 5 seconds
  karate.configure('connectTimeout', 5000);
  karate.configure('readTimeout', 5000);

  var env = karate.env; // get java system property 'karate.env'
  karate.log('karate.env system property is set:', env);
  if (!env) {
    env = 'dev'; // a custom 'intelligent' default
  }
  var config = { // base config JSON
    vmAppUrl: 'http://localhost:5000',
    auth0: {
      domain: 'volley-mgmt-dev',
      clientId: 'A4tbWQojYmGgmxiFcA21KeoFbnG4c5Ex',
      audience: 'https://api.dev.volley-mgmt.org.ua',
      clientSecret: java.lang.System.getenv('VM_KARATE_AUTH0_CLIENT_SECRET')
    }
  };

  if (env === 'staging') {
    config.vmAppUrl = 'https://staging-volley-mgmt.azurewebsites.net/';
    config.auth0.domain = 'volley-mgmt-staging';
    config.auth0.clientId = 'gHFwhskwh3HLP7fYobiUFSW3BK8055DL';
    config.auth0.audience = 'https://api.staging.volley-mgmt.org.ua';
  }

  // Authenticate request
  var result = karate.callSingle('authenticate.feature', config);
  config.auth_header = 'Bearer ' + result.access_token;

  return config;
}