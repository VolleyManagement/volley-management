var VM = (function() {

    'use strict';

    var privates = {};
    
    privates.getNamespace = function(namespace, createIfNotExist, basicNamespace) {

        var currentNs,
            nsNames,
            currentName,
            i;

        if (namespace) {
            nsNames = namespace.split('.');
            currentNs = basicNamespace || VM;

            for (i = 0; i < nsNames.length; i++) {
                currentName = nsNames[i];
                if (!currentNs[currentName]) {
                    if (createIfNotExist) {
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
        
        getNamespace: function(namespace, createIfNotExist) {
            return privates.getNamespace(namespace, createIfNotExist);
        },

        addNamespace: function(namespace) {
            return privates.getNamespace(namespace, true);
        }
    };

})();

$(document).ready(VM.execDocReadyHandlers);