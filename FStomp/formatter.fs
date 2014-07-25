namespace fstomp

module Formatter =         

    let host h = ("host", h)
    let text = ("content-type", "text/plain")
    let channel dst = ("destination", dst)
    let id id = ("id", id)
    let acceptVersion = ("accept-version", "1.2")
    let heartbeat = ("heartbeat", "10000,10000")
    let transaction tid = ("transaction", tid)           

    let formatHeader (name, value) = sprintf "%s:%s" name value

    let formatImp c headers body = 
        let formatHeaders hs = (Seq.map formatHeader headers) |> String.concat "\n"
        sprintf "[\"%s\n%s\n%s\n\0\"]" c (formatHeaders headers) body
        |> utils.translate [ ("\n", "\\n")
                             ("\0", "\\u0000")]                                 
    let format = function        
        | Send (ch, body) -> formatImp "SEND" [channel ch ; text ] body        
        | Subscribe (sid, ch) -> formatImp "SUBSCRIBE" [id sid; channel ch ] ""
        | Unsubscribe ch -> formatImp "UNSUBSCRIBE" [channel ch] ""
        | Begin tid -> formatImp "BEGIN" [transaction tid] ""
        | Commit tid -> formatImp "COMMIT" [transaction tid] ""
        | Abort tid -> formatImp "ABORT" [transaction tid] ""
        | Ack (mid, tid) -> formatImp "ACK" [id mid; transaction tid] ""
        | Nack (mid, tid) -> formatImp "NACK" [id mid; transaction tid] ""
        | Disconnect -> formatImp "DISCONNECT" [] ""  // TODO: receipt id
        | Connect (hostname, hb) -> formatImp 
                                        "CONNECT"
                                        ([acceptVersion; host hostname ]
                                            |> utils.consp hb heartbeat)
                                        ""        
        | _ -> failwith "not implemented"
        