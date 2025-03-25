import { UnityControl } from "./unity-control.interface";

export interface IMessage {
  serializer(): string;
  deserializer?(): UnityControl;
}