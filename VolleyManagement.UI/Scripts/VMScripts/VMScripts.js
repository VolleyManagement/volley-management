var VM = (function() {

    'use strict';

    var localizationResources = {},
        privates = {};

    privates.getNamespace = function(namespace, createIfNotExist, basicNamespace) {

        var create = createIfNotExist || false,
            currentNs,
            nsNames,
            currentName,
            i;

        if (namespace) {
            nsNames = namespace.split('.');
            currentNs = basicNamespace || VM;

            for (i = 0; i < nsNames.length; i++) {
                currentName = nsNames[i];
                if (!currentNs[currentName]) {
                    if (create) {
                        currentNs[currentName] = {};
                    } else {
                        currentNs = undefined;
                        break;
                    }
                }
                currentNs = currentNs[currentName];
            }
        }

        return currentNs;
    };

    return {

        addNamespace: function(namespace) {
            return privates.getNamespace(namespace, true);
        },

        addTranslation: function(key, value, namespace) {
            var currentNs = localizationResources;

            if (namespace) {
                currentNs = privates.getNamespace(namespace, true, localizationResources);
            }

            currentNs[key] = value;
        },

        TR: function(key, namespace) {
            var currentNs = localizationResources,
                result;

            if (key) {
                if (namespace) {
                    currentNs = privates.getNamespace(namespace, false, currentNs);
                }
                result = currentNs[key];
            }

            return result;
        }
    };

})();
