function fn() {
  var env = karate.env; // get java system property 'karate.env'
  karate.log('karate.env system property is set:', env);
  if (!env) {
    env = 'dev'; // a custom 'intelligent' default
  }
  var config = { // base config JSON
    vmAppUrl: 'http://localhost:5000'
  };

  if (env === 'integration') {
    config.vmAppUrl = 'https://staging-volley-mgmt.azurewebsites.net/';
  }

  // don't waste time waiting for a connection or if servers don't respond within 5 seconds
  karate.configure('connectTimeout', 5000);
  karate.configure('readTimeout', 5000);
  return config;
}