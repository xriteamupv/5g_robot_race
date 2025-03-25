export interface RosTopics {
  subscriber: RosTopic,
  control: RosTopic
}

export interface RosTopic {
  name : string,
  messageType : string
}