var rceWorker = {};
(async _rceWorker => {
    let baseUrl = 'http://localhost:3140';

    class UrlSuffixes {
        static get registerAddressSuffix() {
            return '/api/workers/register/'
        };
        static get getJobAddressSuffix() {
            return '/api/workers/WORKER_ID/jobs/'
        };
        static get updateJobAddressSuffix() {
            return '/api/workers/WORKER_ID/jobs/JOB_ID/update'
        };
        static get completeJobAddressSuffix() {
            return '/api/workers/WORKER_ID/jobs/JOB_ID/complete'
        };
        static get closeWorkerAddressSuffix() {
            return '/api/workers/WORKER_ID/close'
        };
    }

    class JobRunnerStateMachine {
        stopped = false;
        workerId = '';
        state = {};

        async start() {
            this.stopped = false;
            this.state = new RegistrationState();
            while (this.stopped === false) {
                await this.state.handle(this);
                await new Promise(e => setTimeout(e, 10000));
            }
        }
        async stop() {
            this.stopped = true;
            this.closeWorker();
        }
        async closeWorker() {
            //todo
        }
    }

    class RegistrationState {
        registrationModel = {
            name: 'Counter',
            description: 'JS Worker',
            //base64Logo: '',
            jobDescriptions: [{
                name: 'Count',
                description: [
                    'Counts in one second intervals. Params:',
                    ' - from [int]',
                    ' - to [int]'
                ],
                defaultPayload: {
                    'from': 0,
                    'to': 5
                }
            }]
        };
		
        async handle(jobRunnerStateMachine) {
            console.log('Registering worker');
            const url = baseUrl + UrlSuffixes.registerAddressSuffix;
            const response = await fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(this.registrationModel)
            }).then(e => e.text());
            console.log(response);
        }
    }

    let jobRunnerStateMachine = new JobRunnerStateMachine();
    _rceWorker.start = () => jobRunnerStateMachine.start();
    _rceWorker.stop = () => jobRunnerStateMachine.stop();
    _rceWorker.start(); // not awaiting this
})(rceWorker);