var VM = (function () {

    return {

        addNamespace: function (namespaceName) {

            var nsNames = namespaceName.split('.'),
                currentNs = VM,
                currentName;

            for (var i = 0; i < nsNames.length; i++) {
                currentName = nsNames[i];
                if (!currentNs[currentName]) {
                    currentNs[currentName] = {};
                }
                currentNs = currentNs[currentName];
            }

            return currentNs;
        }
    }
})();