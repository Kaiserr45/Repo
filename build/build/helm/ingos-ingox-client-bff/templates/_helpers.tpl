{{/* vim: set filetype=mustache: */}}
{{/*
Expand the name of the chart.
*/}}
{{- define "application.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "application.fullname" -}}
{{- if .Values.fullnameOverride }}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default .Chart.Name .Values.nameOverride }}
{{- if contains $name .Release.Name }}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}
{{- end }}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "application.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "application.labels" -}}
helm.sh/chart: {{ include "application.chart" . }}
{{ include "application.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "application.selectorLabels" -}}
app.kubernetes.io/name: {{ include "application.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
Create the name of the service account to use
*/}}
{{- define "application.serviceAccountName" -}}
{{- if .Values.serviceAccount.create }}
{{- default (include "application.fullname" .) .Values.serviceAccount.name }}
{{- else }}
{{- default "default" .Values.serviceAccount.name }}
{{- end }}
{{- end }}

{{/*
Calculate PDB. 
The PDB value MUST be lower than minimum requested replicas of your app to prevent locking out drains during node maintenance.
По-русски:
Значение PDB должно быть меньше, чем минимальное запрошенное в кластере количество реплик вашего приложения. В противном случае, кубер не может их переместить с ноды,
когда админ запрашивает выселение для ее обслуживания.
*/}}
{{- define "application.getMinAvaliable" -}}
{{ $desired := int (ternary .Values.autoscaling.minReplicas .Values.replicaCount .Values.autoscaling.enabled) }}
{{- $minAvaliable := sub $desired 1 }}
{{- if and (lt $minAvaliable 1) .Values.podDisruptionBudget.enabled }}
{{- fail "ERROR! Calculated value of minAvaliable replicas for PodDisruptionBudget is lower than 1! You should disable PDB or increase desired replicas of your app! See this function comment for more info." }}
{{- end }}
{{- $minAvaliable }}
{{- end }}

{{- define "imagePullSecrets" }}
{{- $registry := (split "/" .Values.app.image.repository)._0 }}
{{- $username := .Values.app.imageCredentials.username }}
{{- $password := .Values.app.imageCredentials.password }}
{{- $auth := (printf "%s:%s" $username $password) | b64enc }}
{{- printf "{\"auths\":{\"%s\":{\"username\":\"%s\",\"password\":\"%s\",\"auth\":\"%s\"}}}" $registry $username $password $auth | b64enc }}
{{- end }}