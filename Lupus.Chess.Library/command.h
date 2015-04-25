#pragma once

namespace chess {
    class command {
    public:
        virtual ~command() = default;
        virtual void execute() = 0;
        virtual void undo() = 0;
    };
}
