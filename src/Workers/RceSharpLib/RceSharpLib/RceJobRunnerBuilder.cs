using RceSharpLib.JobExecutors;
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
		private List<string> _owners;
		private List<Type> _jobExecutorTypes = new List<Type>();
		private Dictionary<string, Type> _jobExecutorDictionary = new Dictionary<string, Type>();
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

		public RceJobRunnerBuilder SetOwners(List<string> owners)
		{
			_owners = owners;
			return this;
		}

		public RceJobRunnerBuilder AddJobExecutorType(Type jobExecutorType)
		{
			_jobExecutorTypes.Add(jobExecutorType);
			return this;
		}

		public RceJobRunner Build()
		{
			BuildRegistrationModel();
			BuildJobExecutorDictionary();

			return new RceJobRunner
			{
				JobRunnerContext = new JobRunnerContext
				{
					BaseUrl = _baseUrl ?? throw new ArgumentException($"{nameof(JobRunnerContext.BaseUrl)} must be set"),
					Owners = _owners ?? throw new ArgumentException($"{nameof(JobRunnerContext.Owners)} must be set"),
					JobExecutorDictionary = _jobExecutorDictionary ?? throw new ArgumentException($"{nameof(JobRunnerContext.JobExecutorDictionary)} must be set"),
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
				Base64Logo = _workerBase64Logo ?? throw new ArgumentException($"{nameof(RegistrationModel.Base64Logo)} must be set"),
				JobDescriptions = BuildJobDescriptions(),
				Owners = _owners
			};
		}

		private List<JobDescription> BuildJobDescriptions()
		{
			var jobDescriptions = new List<JobDescription>();
			foreach (var jobExecutorType in _jobExecutorTypes)
			{
				if (typeof(JobExecutorBase).IsAssignableFrom(jobExecutorType) == false)
				{
					throw new ArgumentException($"'{jobExecutorType}' must derive from '{nameof(JobExecutorBase)}'");
				}

				var jobExecutor = (JobExecutorBase)Activator.CreateInstance(jobExecutorType, new object[] { null, null });
				jobDescriptions.Add(jobExecutor.JobDescription);
			}

			return jobDescriptions;
		}

		private void BuildJobExecutorDictionary()
		{
			foreach (var jobExecutorType in _jobExecutorTypes)
			{
				if (typeof(JobExecutorBase).IsAssignableFrom(jobExecutorType) == false)
				{
					throw new ArgumentException($"'{jobExecutorType}' must derive from '{nameof(JobExecutorBase)}'");
				}

				var jobExecutor = (JobExecutorBase)Activator.CreateInstance(jobExecutorType, new object[] { null, null });
				_jobExecutorDictionary.Add(jobExecutor.JobDescription.Name, jobExecutorType);
			}
		}
	}
}
