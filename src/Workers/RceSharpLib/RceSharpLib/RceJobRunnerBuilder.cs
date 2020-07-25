using RceSharpLib.JobHandlers;
using System;
using System.Collections.Generic;

namespace RceSharpLib
{
	public class RceJobRunnerBuilder
	{
		private string _baseUrl;
		private string _workerName;
		private string _workerDescription;
		private string _workerBase64Logo;
		private bool _runInParallel = true;
		private List<string> _owners;
		private List<Type> _jobHandlerTypes = new List<Type>();
		private Dictionary<string, Type> _jobHandlerDictionary = new Dictionary<string, Type>();
		private RegistrationModel _registrationModel;

		public RceJobRunnerBuilder SetBaseUrl(string baseUrl)
		{
			_baseUrl = baseUrl;
			return this;
		}

		public RceJobRunnerBuilder SetWorkerName(string workerName)
		{
			_workerName = workerName;
			return this;
		}

		public RceJobRunnerBuilder SetWorkerDescription(string workerDescription)
		{
			_workerDescription = workerDescription;
			return this;
		}

		public RceJobRunnerBuilder SetWorkerBase64Logo(string workerBase64Logo)
		{
			_workerBase64Logo = workerBase64Logo;
			return this;
		}

		public RceJobRunnerBuilder SetRunInParallel(bool runInParallel)
		{
			_runInParallel = runInParallel;
			return this;
		}

		public RceJobRunnerBuilder SetOwners(List<string> owners)
		{
			_owners = owners;
			return this;
		}

		public RceJobRunnerBuilder AddJobHandlerType(Type jobHandlerType)
		{
			_jobHandlerTypes.Add(jobHandlerType);
			return this;
		}

		public RceJobRunner Build()
		{
			BuildRegistrationModel();
			BuildJobHandlerDictionary();

			return new RceJobRunner
			{
				JobRunnerContext = new JobRunnerContext
				{
					BaseUrl = _baseUrl ?? throw new ArgumentException($"{nameof(JobRunnerContext.BaseUrl)} must be set"),
					Owners = _owners ?? throw new ArgumentException($"{nameof(JobRunnerContext.Owners)} must be set"),
					RunInParallel = _runInParallel,
					JobHandlerDictionary = _jobHandlerDictionary ?? throw new ArgumentException($"{nameof(JobRunnerContext.JobHandlerDictionary)} must be set"),
					RegistrationModel = _registrationModel ?? throw new ArgumentException($"{nameof(JobRunnerContext.RegistrationModel)} must be set"),
				}
			};
		}

		private void BuildRegistrationModel()
		{
			_registrationModel = new RegistrationModel
			{
				Name = _workerName ?? throw new ArgumentException($"{nameof(RegistrationModel.Name)} must be set"),
				Description = _workerDescription ?? throw new ArgumentException($"{nameof(RegistrationModel.Description)} must be set"),
				Base64Logo = _workerBase64Logo,
				JobDescriptions = BuildJobDescriptions(),
				Owners = _owners
			};
		}

		private List<JobDescription> BuildJobDescriptions()
		{
			var jobDescriptions = new List<JobDescription>();
			foreach (var jobHandlerType in _jobHandlerTypes)
			{
				if (typeof(JobHandlerBase).IsAssignableFrom(jobHandlerType) == false)
				{
					throw new ArgumentException($"'{jobHandlerType}' must derive from '{nameof(JobHandlerBase)}'");
				}

				var jobHandler = (JobHandlerBase)Activator.CreateInstance(jobHandlerType, new object[] { null, null });
				jobDescriptions.Add(jobHandler.JobDescription);
			}

			return jobDescriptions;
		}

		private void BuildJobHandlerDictionary()
		{
			foreach (var jobHandlerType in _jobHandlerTypes)
			{
				if (typeof(JobHandlerBase).IsAssignableFrom(jobHandlerType) == false)
				{
					throw new ArgumentException($"'{jobHandlerType}' must derive from '{nameof(JobHandlerBase)}'");
				}

				var jobHandler = (JobHandlerBase)Activator.CreateInstance(jobHandlerType, new object[] { null, null });
				_jobHandlerDictionary.Add(jobHandler.JobDescription.Name, jobHandlerType);
			}
		}
	}
}
