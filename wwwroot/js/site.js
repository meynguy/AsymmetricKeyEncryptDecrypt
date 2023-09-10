// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// Function to generate a key pair
async function generateEncryptionKeyPair() {
    const keyPair = await window.crypto.subtle.generateKey(
        {
            name: "RSA-OAEP",
            modulusLength: 2048,
            publicExponent: new Uint8Array([0x01, 0x00, 0x01]), // 65537
            hash: { name: "SHA-256" },
        },
        true,
        ["encrypt", "decrypt"]
    );

    return keyPair;
}

// Function to export the public key
async function exportPublicKey(keyPair) {
    const publicKey = await crypto.subtle.exportKey('jwk', keyPair.publicKey);
    return publicKey;
}

// Function to export the private key
async function exportPrivateKey(keyPair) {
    const privateKey = await crypto.subtle.exportKey('jwk', keyPair.privateKey);
    return privateKey;
}


// Function to import the public key
async function importEncryptionPublicKey(publicKeyData) {
    const publicKey = await crypto.subtle.importKey(
        "jwk",
        publicKeyData,
        {
            name: "RSA-OAEP",
            hash: { name: "SHA-256" },
        },
        true,
        ["encrypt"]
    );

    return publicKey;
}

// Function to import the private key
async function importDecryptionPrivateKey(privateKeyData) {
    const privateKey = await crypto.subtle.importKey(
        "jwk",
        privateKeyData,
        {
            name: "RSA-OAEP",
            hash: { name: "SHA-256" },
        },
        true,
        ["decrypt"]
    );

    return privateKey;
}

// Function to encrypt a message
async function encryptMessage(message, publicKey) {
    const encodedMessage = new TextEncoder().encode(message);
    const encrypted = await crypto.subtle.encrypt(
        {
            name: "RSA-OAEP",
        },
        publicKey,
        encodedMessage
    );
    return new Uint8Array(encrypted);
}

// Function to decrypt a message
async function decryptMessage(ciphertext, privateKey) {
    const decrypted = await crypto.subtle.decrypt(
        {
            name: "RSA-OAEP",
        },
        privateKey,
        ciphertext
    );
    const decodedMessage = new TextDecoder().decode(decrypted);
    return decodedMessage;
}

async function main() {
    var keyPair = await generateEncryptionKeyPair();
    var publicKeyData = await exportPublicKey(keyPair);
    var privateKeyData = await exportPrivateKey(keyPair);
    localStorage.setItem("ED-privateKey", JSON.stringify(privateKeyData));

    localStorage.setItem('ED-publicKey', JSON.stringify(publicKeyData));
    const storedPublicKeyData = JSON.parse(localStorage.getItem("ED-publicKey"));
    const importedPublicKey = await importEncryptionPublicKey(storedPublicKeyData);
    const message = 'Hello, SubtleCrypto!';
    var messageCipher = await encryptMessage(message, importedPublicKey);

    const importedPrivateKey = await importDecryptionPrivateKey(JSON.parse(localStorage.getItem("ED-privateKey")));
    if (await decryptMessage(messageCipher, importedPrivateKey) === message) {
        fetch('storePublicKey', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                publicKeyAsBase64: btoa(JSON.stringify(publicKeyData))
            })
        }).then(response => response.json())
            .then(data => {
                console.log(data);
                decryptMessage(data.EncryptedMessage, importedPrivateKey).then(plainText => {
                    console.log(plainText);
                });
                
            })
            .catch(error => {
                console.error('Error:', error);
            });
    } else {
        console.error("Failure");
    }
}

// Execute the main function
main();