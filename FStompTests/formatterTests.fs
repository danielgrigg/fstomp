module formatter.Tests

    open NUnit.Framework
    open FsUnit
    open fstomp 
 
 
    [<Test>]
    let ``SEND`` ()=
        Send ("/queue/foo", "Simple body") |> Formatter.format |> should equal 
            "[\"SEND\\ndestination:/queue/foo\\ncontent-type:text/plain\\nSimple body\\n\\u0000\"]";

    [<Test>]
    let ``SUBSCRIBE`` ()=
         Subscribe ("s123", "/queue/foo") |> Formatter.format |> should equal 
            "[\"SUBSCRIBE\\nid:s123\\ndestination:/queue/foo\\n\\n\\u0000\"]";

    [<Test>]
    let ``UNSUBSCRIBE`` ()=        
        Unsubscribe "s123" |> Formatter.format |> should equal "[\"UNSUBSCRIBE\\ndestination:s123\\n\\n\\u0000\"]"

    [<Test>]
    let ``BEGIN`` ()=
        Begin ("t123") |> Formatter.format |> should equal "[\"BEGIN\\ntransaction:t123\\n\\n\\u0000\"]"

    [<Test>]
    let ``COMMIT`` ()=        
        Commit "t123" |> Formatter.format  |> should equal "[\"COMMIT\\ntransaction:t123\\n\\n\\u0000\"]"
        
    [<Test>]
    let ``ABORT`` ()=        
        Abort ("t123") |> Formatter.format |> should equal "[\"ABORT\\ntransaction:t123\\n\\n\\u0000\"]"

    [<Test>]
    let ``ACK`` ()=
        Ack ("m123", "t123") |> Formatter.format |> should equal "[\"ACK\\nid:m123\\ntransaction:t123\\n\\n\\u0000\"]"

    [<Test>]
    let ``NACK`` ()=
        Nack ("mid", "tid") |> Formatter.format |> should equal "[\"NACK\\nid:mid\\ntransaction:tid\\n\\n\\u0000\"]"

    [<Test>]
    let ``DISCONNECT`` ()=
        Formatter.format Disconnect |> should equal "[\"DISCONNECT\\n\\n\\n\\u0000\"]"

    [<Test>]
    let ``CONNECT`` ()=        
        Connect ("host.at.nowhere", true) |> Formatter.format |> should equal 
            "[\"CONNECT\\nheartbeat:10000,10000\\naccept-version:1.2\\nhost:host.at.nowhere\\n\\n\\u0000\"]"
