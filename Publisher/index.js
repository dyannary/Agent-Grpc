import { loadPackageDefinition, credentials } from '@grpc/grpc-js';
import { loadSync } from "@grpc/proto-loader";
import path from "path";
import readline from 'readline';

const protoFilePath = './Protos/publisher.proto';

const packageDefinition = loadSync(
    path.resolve(protoFilePath),
    {
        keepCase: true,
        longs: String,
        enums: String,
        defaults: true,
        oneofs: true,
    }
);

const publisherProto = loadPackageDefinition(packageDefinition).GrpcsAgent;
const brokerAddress = '192.168.1.119:5001';
const client = new publisherProto.Publisher(
    brokerAddress,
    credentials.createInsecure()
);

function publishMessage(topic, message) {
    const request = {
        topic,
        message,
    };

    client.PublishMessage(request, (error, response) => {
        if (!error && response && response.isSuccess) {
            console.log('Message published successfully.');
            console.log(request)
        } else {
            console.error('Error publishing message:', error || 'Unknown error occurred');
        }
        readInput();
    });
}

const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

function getSavedTopics() {
    const request = {};

    client.GetSavedTopics(request, (error, response) => {
        if (!error && response && response.topics) {
            console.log('Saved Topics:');
            console.log(response.topics);
        } else {
            console.error('Error retrieving topics:', error || 'Unknown error occurred');
        }

        readInput();
    });
}

function readInput() {
    rl.question('Enter the topic (type "exit" to quit, "topics" to see saved topics): ', (input) => {
        if (!input) {
            console.error('Invalid input. Please enter a topic.');
            readInput();
            return;
        }

        const lowercaseInput = input.toLowerCase();

        if (lowercaseInput === 'exit') {
            rl.close();
        } else if (lowercaseInput === 'topics') {
            getSavedTopics();
        } else {
            rl.question('Enter the message message: ', (messagemessage) => {
                if (!messagemessage) {
                    console.error('Invalid message message. Please enter a message message.');
                    readInput();
                    return;
                }
                publishMessage(input, messagemessage);
            });
        }
    });
}

readInput();
