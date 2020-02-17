var rceWorker = {};
(async _rceWorker => {
    class RegistrationState {
        async handle(jobRunnerStateMachine) {
            console.log('Registering worker');
        }
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
                await new Promise(e => setTimeout(e, 1000));
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

    let jobRunnerStateMachine = new JobRunnerStateMachine();
    _rceWorker.start = () => jobRunnerStateMachine.start();
    _rceWorker.stop = () => jobRunnerStateMachine.stop();
    _rceWorker.start(); // not awaiting this
})(rceWorker);