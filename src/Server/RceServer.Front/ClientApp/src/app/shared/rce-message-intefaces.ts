export interface RceMessage {
    messageId: string;
    messageTimestamp: number;
    messageType: string;
}

export interface JobDescription extends RceMessage {
    name: string;
    description: string[];
    defaultPayload: any;
}

export interface JobAddedMessage extends RceMessage {
    jobId: string;
    workerId: string;
    name: string;
    payload: any;
}

export enum Statuses {
    Undefined,
    Success,
    Warning,
    Failure,
}

export interface JobCompletedMessage extends RceMessage {
    jobId: string;
    workerId: string;
    jobStatus: Statuses;
    output: any;
}

export interface JobPickedUpMessage extends RceMessage {
    jobId: string;
    workerId: string;
}

export interface JobRemovedMessage extends RceMessage {
    jobId: string;
    workerId: string;
}

export interface JobUpdatedMessage extends RceMessage {
    jobId: string;
    workerId: string;
    output: any;
}

export interface WorkerAddedMessage extends RceMessage {
    workerId: string;
    name: string;
    description: string;
    base64Logo: string;
    jobDescriptions: JobDescription[];
}

export interface WorkerRemovedMessage extends RceMessage {
    workerId: string;
    connectionStatus: string;
}
