var rceWorker = {};
(async _rceWorker => {
  let baseUrl = "https://rceserver.azurewebsites.net";
  let owners = ["demo@example.com"];
  let registrationModel = {
    name: "JS Counter",
    description: "JS Worker Example",
    base64Logo: "data:image/png;base64,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",
    jobDescriptions: [
      {
        name: "Count",
        description: [
          "Counts in one second intervals. Params:",
          " - from [int]",
          " - to [int]"
        ],
        defaultPayload: {
          from: 0,
          to: 5
        }
      }
    ],
    owners: owners
  };

  class UrlSuffixes {
    static get registerAddressSuffix() {
      return "/api/workers/register/";
    }
    static get getJobsAddressSuffix() {
      return "/api/workers/WORKER_ID/jobs/";
    }
    static get updateJobAddressSuffix() {
      return "/api/workers/WORKER_ID/jobs/JOB_ID/update";
    }
    static get completeJobAddressSuffix() {
      return "/api/workers/WORKER_ID/jobs/JOB_ID/complete";
    }
    static get closeWorkerAddressSuffix() {
      return "/api/workers/WORKER_ID/close";
    }
  }

  class JobStatuses {
    static get success() {
      return "Success";
    }
    static get warning() {
      return "Warning";
    }
    static get failure() {
      return "Failure";
    }
  }

  class JobRunner {
    running = false;
    workerId = "";
    state = {};

    async start() {
      this.running = true;
      this.state = new RegistrationState();
      while (this.running === true) {
        try {
          await this.state.handle(this);
        } catch (error) {
          await new Promise(e => setTimeout(e, 5000));
          this.state = new RegistrationState();
        }
        await new Promise(e => setTimeout(e, 1));
      }
    }

    async stop() {
      this.running = false;
      this.closeWorker();
    }

    async closeWorker() {
      try {
        const url = baseUrl + UrlSuffixes.closeWorkerAddressSuffix.replace(
          "WORKER_ID",
          this.workerId
        );
        await fetch(url, {
          method: "POST"
        });
      } catch (error) {
        // ignore
      }
    }
  }

  class RegistrationState {
    registrationModel = registrationModel;

    async handle(jobRunner) {
      try {
        console.log("Registering worker");
        const url = baseUrl + UrlSuffixes.registerAddressSuffix;
        const response = await fetch(url, {
          method: "POST",
          headers: {
            "Content-Type": "application/json"
          },
          body: JSON.stringify(this.registrationModel)
        })
          .then(e => validateResponse(e))
          .then(e => e.text());
        jobRunner.workerId = response.split('"').join("");
        jobRunner.state = new GetJobsState();
      } catch (error) {
        jobRunner.state = new FailedState(error);
      }
    }
  }

  class GetJobsState {
    async handle(jobRunner) {
      try {
        console.log("Getting jobs");
        const url = baseUrl + UrlSuffixes.getJobsAddressSuffix.replace(
          "WORKER_ID",
          jobRunner.workerId
        );
        const response = await fetch(url)
          .then(e => validateResponse(e))
          .then(e => e.json());

        if (response.length == 0) {
          jobRunner.state = new GetJobsState();
          return;
        }

        response.forEach(e => (e.workerId = jobRunner.workerId));
        jobRunner.state = new RunJobsState(response);
      } catch (error) {
        jobRunne.state = new FailedState(error);
      }
    }
  }

  class RunJobsState {
    jobs = [];

    constructor(jobs) {
      this.jobs = jobs;
    }

    async handle(jobRunner) {
      try {
        for (let job of this.jobs) {
          switch (job.jobName) {
            case "Count":
              new CountJobHandler(job).run();
              break;

            default:
              await new JobHandlerBase(job).failJob(
                `No job named '${job.jobName}'`
              );
          }
        }
        jobRunner.state = new GetJobsState();
      } catch (error) {
        jobRunner.state = new FailedState(error);
      }
    }
  }

  class FailedState {
    failureReason = "";

    constructor(failureReason) {
      this.failureReason = failureReason;
    }

    async handle(jobRunner) {
      console.log(`Problem encountered: '${this.failureReason}'`);
      await new Promise(e => setTimeout(e, 5000));
      jobRunner.state = new RegistrationState();
    }
  }

  class JobHandlerBase {
    job = {};

    constructor(job) {
      this.job = job;
    }

    async failJob(reason) {
      console.log(`Job failed: '${this.job.jobName}' '${this.job.jobId}' '${reason}'`);
      try {
        const url = baseUrl + UrlSuffixes.completeJobAddressSuffix
          .replace("WORKER_ID", this.job.workerId)
          .replace("JOB_ID", this.job.jobId);
        await fetch(url, {
          method: "POST",
          headers: {
            "Content-Type": "application/json"
          },
          body: JSON.stringify({
            failure: reason,
            jobStatus: JobStatuses.failure
          })
        }).then(e => validateResponse(e));
      } catch (error) {
        // ignore
      }
    }

    async updateJob(payload) {
      console.log(`Updating job: '${this.job.jobName}' '${this.job.jobId}'`);
      try {
        const url = baseUrl + UrlSuffixes.updateJobAddressSuffix
          .replace("WORKER_ID", this.job.workerId)
          .replace("JOB_ID", this.job.jobId);
        await fetch(url, {
          method: "POST",
          headers: {
            "Content-Type": "application/json"
          },
          body: JSON.stringify(payload)
        }).then(e => validateResponse(e));
      } catch (error) {
        // ignore
      }
    }

    async completeJob(payload) {
      console.log(`Completing job: '${this.job.jobName}' '${this.job.jobId}'`);
      try {
        const url = baseUrl + UrlSuffixes.completeJobAddressSuffix
          .replace("WORKER_ID", this.job.workerId)
          .replace("JOB_ID", this.job.jobId);
        await fetch(url, {
          method: "POST",
          headers: {
            "Content-Type": "application/json"
          },
          body: JSON.stringify(payload)
        }).then(e => validateResponse(e));
      } catch (error) {
        await this.failJob(error);
      }
    }
  }

  class CountJobHandler extends JobHandlerBase {
    job = {};

    constructor(job) {
      super(job);
      this.job = job;
    }

    async run() {
      console.log(`Running job: '${this.job.jobName}' '${this.job.jobId}'`);
      try {
        let from = this.job.payload.from;
        let to = this.job.payload.to;
        for (let i = from; i < to; i++) {
          await this.updateJob({
            currentCount: i
          });
          await new Promise(e => setTimeout(e, 1000));
        }

        await this.completeJob({
          currentCount: to,
          jobStatus: JobStatuses.success
        });
      } catch (error) {
        await this.failJob(error);
      }
    }
  }

  function validateResponse(response) {
    if (response.status !== 200) {
      throw `Invalid response code: '${response.status}'`;
    }
    return response;
  }

  let jobRunner = new JobRunner();
  _rceWorker.start = () => jobRunner.start();
  _rceWorker.stop = () => jobRunner.stop();
  _rceWorker.start(); // not awaiting this
})(rceWorker);
