export enum JobStates {
    Added = 'Added',
    PickedUp = 'PickedUp',
    Updated = 'Updated',
    Success = 'Success',
    Warning = 'Warning',
    Failure = 'Failure',
}

export class Job {
    jobId: string;
    workerId: string;
    name: string;
    payload: any;
    output: any;
    jobState: JobStates;
}
