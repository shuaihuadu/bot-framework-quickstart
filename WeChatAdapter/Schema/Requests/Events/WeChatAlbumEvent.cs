﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Xml.Serialization;

namespace WeChatAdapter.Schema.Requests.Events;

[XmlRoot("xml")]
public class WeChatAlbumEvent : RequestEventWithEventKey
{
    public override string EventType => EventTypes.WeChatAlbum;

    [XmlElement(ElementName = "SendPicsInfo")]
    public SendPicsInfo? SendPicsInfo { get; set; }
}
